using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Castle.Core;
using Castle.Windsor;
using MinaGlosor.Web;
using MinaGlosor.Web.Controllers.Api;
using MinaGlosor.Web.Infrastructure.IoC;
using NUnit.Framework;
using Raven.Client;

namespace MinaGlosor.Test.Api
{
    public abstract class WebApiIntegrationTest
    {
        protected HttpClient Client { get; private set; }

        private IWindsorContainer Container { get; set; }

        [SetUp]
        public void SetUp()
        {
            var configuration = new HttpConfiguration();
            Container = new WindsorContainer();
            Container.Install(
                new ControllerInstaller(),
                new WindsorWebApiInstaller(),
                new ControllerFactoryInstaller(),
                new RavenInstaller(ApplicationMode.Test, LifestyleType.Scoped),
                new HandlersInstaller());
            OnSetUp(Container);

            MvcApplication.Configure(Container, configuration);
            Client = new HttpClient(new HttpServer(configuration));

            CustomAuthorizeAttribute.DisableAuthorize = true;
        }

        [TearDown]
        public void TearDown()
        {
            OnTearDown();
            MvcApplication.Shutdown();
        }

        protected void Transact(Action<IDocumentSession> action)
        {
            using (var session = Container.Resolve<IDocumentStore>().OpenSession())
            {
                action.Invoke(session);
                session.SaveChanges();
            }

            WaitForIndexing();
        }

        protected virtual void OnSetUp(IWindsorContainer container)
        {
        }

        protected virtual void OnTearDown()
        {
        }

        private void WaitForIndexing()
        {
            var documentStore = Container.Resolve<IDocumentStore>();
            var indexingTask = Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {
                        var s = documentStore.DatabaseCommands.GetStatistics().StaleIndexes;
                        if (s.Length == 0)
                        {
                            break;
                        }

                        Task.Delay(500);
                    }
                });
            indexingTask.Wait(15000);
        }
    }
}