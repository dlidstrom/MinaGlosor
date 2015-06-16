using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using MinaGlosor.Web;
using MinaGlosor.Web.Infrastructure.IoC.Installers;
using NUnit.Framework;
using Raven.Client;

namespace MinaGlosor.Test.Api
{
    public abstract class WebApiIntegrationTest : ExceptionLogger
    {
        protected HttpClient Client { get; private set; }

        private IWindsorContainer Container { get; set; }

        public override void Log(ExceptionLoggerContext context)
        {
            Assert.Fail(context.Exception.ToString());
        }

        [SetUp]
        public void SetUp()
        {
            var configuration = new HttpConfiguration();
            configuration.Services.Add(typeof(IExceptionLogger), this);
            Container = new WindsorContainer();
            Container.Install(
                RavenInstaller.CreateForTests(),
                new WindsorWebApiInstaller(),
                new HandlersInstaller(),
                new AdminCommandHandlerInstaller());
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("e@d.com"), new string[0]);
            OnSetUp(Container);

            Application.Bootstrap(Container, configuration);
            Client = new HttpClient(new HttpServer(configuration));

            Arrange();
            Act();
        }

        [TearDown]
        public void TearDown()
        {
            OnTearDown();
            Application.Shutdown();
        }

        protected virtual void Arrange()
        {
        }

        protected virtual void Act()
        {
        }

        protected void Transact(Action<IDocumentSession> action)
        {
            WaitForIndexing();

            using (Container.BeginScope())
            using (var session = Container.Resolve<IDocumentSession>())
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

        protected void WaitForIndexing()
        {
            var documentStore = Container.Resolve<IDocumentStore>();
            const int Timeout = 15000;
            var indexingTask = Task.Factory.StartNew(
                () =>
                {
                    var sw = Stopwatch.StartNew();
                    while (sw.Elapsed.TotalMilliseconds < Timeout)
                    {
                        var s = documentStore.DatabaseCommands.GetStatistics()
                                             .StaleIndexes;
                        if (s.Length == 0)
                        {
                            break;
                        }

                        Task.Delay(500).Wait(15000);
                    }
                });
            indexingTask.Wait(Timeout);
        }
    }
}