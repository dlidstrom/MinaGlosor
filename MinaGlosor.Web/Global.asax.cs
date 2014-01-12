using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using MinaGlosor.Web.App_Start;
using MinaGlosor.Web.Infrastructure.IoC;

namespace MinaGlosor.Web
{
    public class MvcApplication : HttpApplication
    {
        private static IWindsorContainer container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            CreateContainer();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.IsSecureConnection.Equals(true)
                || HttpContext.Current.Request.IsLocal.Equals(true)) return;
            var url = string.Format(
                "https://{0}{1}",
                Request.ServerVariables["HTTP_HOST"],
                HttpContext.Current.Request.RawUrl);
            Response.Redirect(url);
        }

        private static void CreateContainer()
        {
            container = new WindsorContainer().Install(
                new ControllerInstaller(),
                new WindsorWebApiInstaller(),
                new ControllerFactoryInstaller(),
                new RavenInstaller());

            DependencyResolver.SetResolver(new WindsorMvcDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver =
                new WindsorHttpDependencyResolver(container.Kernel);
        }
    }
}