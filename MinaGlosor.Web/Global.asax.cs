using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using MinaGlosor.Web.Data.Events;
using MinaGlosor.Web.Infrastructure.IoC;

namespace MinaGlosor.Web
{
    public class MvcApplication : HttpApplication
    {
        private static IWindsorContainer container;

        private static ApplicationMode applicationMode =
#if DEBUG
 ApplicationMode.Debug;

#else
            ApplicationMode.Release;
#endif

        public static ApplicationMode Mode { get { return applicationMode; } }

        public static void Configure(IWindsorContainer windsorContainer, HttpConfiguration configuration)
        {
            container = windsorContainer;
            Configure(configuration);
        }

        public static void Shutdown()
        {
            GlobalFilters.Filters.Clear();
            RouteTable.Routes.Clear();
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
                /*new RavenInstaller(),*/
                new HandlersInstaller(),
                new ContextInstaller());
        }
    }
}