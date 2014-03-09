using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using MinaGlosor.Web.Data.Events;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.IoC;

// TODO: Länken öva ska gå till sida som listar övningar eller ger alternativ att skapa ny
// TODO: Inbjudan måste ha route
// TODO: Inbjudningsmail måste ha länk som har route
// TODO: Acceptera inbjudan genom mail måste ha route
// TODO: Dela upp AccountController
namespace MinaGlosor.Web
{
    public class MvcApplication : HttpApplication
    {
        private static IWindsorContainer container;

        public static void Configure(IWindsorContainer windsorContainer, HttpConfiguration configuration)
        {
            container = windsorContainer;
            Configure(configuration);
        }

        public static void Shutdown()
        {
            GlobalFilters.Filters.Clear();
            RouteTable.Routes.Clear();
            ModelBinders.Binders.Clear();
            if (container != null)
                container.Dispose();
            container = null;
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            Configure(GlobalConfiguration.Configuration);
        }

        private static void Configure(HttpConfiguration configuration)
        {
            WebApiConfig.Register(configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(Guid), new GuidBinder());

            if (container == null)
                container = CreateContainer();
            DependencyResolver.SetResolver(new WindsorMvcDependencyResolver(container));
            configuration.DependencyResolver =
                new WindsorHttpDependencyResolver(container.Kernel);
            DomainEvent.SetContainer(container);
        }

        private static IWindsorContainer CreateContainer()
        {
            return new WindsorContainer().Install(
                new ControllerInstaller(),
                new WindsorWebApiInstaller(),
                new ControllerFactoryInstaller(),
                new HandlersInstaller(),
                new ContextInstaller());
        }
    }
}