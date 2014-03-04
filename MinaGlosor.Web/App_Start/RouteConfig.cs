using System.Web.Mvc;
using System.Web.Routing;

// ReSharper disable CheckNamespace
namespace MinaGlosor.Web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Welcome-route",
                "welcome",
                new { controller = "Welcome", action = "Index" });

            // add routes for register scenario

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{*catchall}",
                defaults: new { controller = "Home", action = "Index" });
        }
    }
}