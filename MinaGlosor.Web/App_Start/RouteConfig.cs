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

            // add specific routes
            routes.MapRoute(
                "Logon-route",
                "logon",
                new { controller = "Account", action = "Logon" });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: string.Empty,
                defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
                name: "Catchall",
                url: "{*catchall}",
                defaults: new { controller = "Redirect", action = "Index" });
        }
    }
}