using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using MinaGlosor.Web.App_Start;
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

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            CreateContainer();
        }

        private static void CreateContainer()
        {
            container = new WindsorContainer().Install(
                new ControllerInstaller(),
                new WindsorWebApiInstaller(),
                new ControllerFactoryInstaller(),
                new RavenInstaller(),
                new HandlersInstaller());

            DependencyResolver.SetResolver(new WindsorMvcDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver =
                new WindsorHttpDependencyResolver(container.Kernel);
            DomainEvent.SetContainer(container);
        }
    }
}