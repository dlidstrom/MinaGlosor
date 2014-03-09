using System;
using System.Net.Http;
using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web;
using MinaGlosor.Web.Controllers.Api;
using MinaGlosor.Web.Data;
using MinaGlosor.Web.Infrastructure.IoC;
using NUnit.Framework;

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
            Container.Register(
                Component.For<IDbContext>()
                         .ImplementedBy<InMemoryDbContext>()
                         .LifestyleSingleton());
            Container.Install(
                new ControllerInstaller(),
                new WindsorWebApiInstaller(),
                new ControllerFactoryInstaller(),
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

        protected void Transact(Action<IDbContext> action)
        {
            var context = Container.Resolve<IDbContext>();
            action.Invoke(context);
        }

        protected virtual void OnSetUp(IWindsorContainer container)
        {
        }

        protected virtual void OnTearDown()
        {
        }
    }
}