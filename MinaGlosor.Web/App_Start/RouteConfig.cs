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

            routes.MapRoute(
                "AccountInvite-route",
                "invite",
                new { controller = "Account", action = "Invite" });

            routes.MapRoute(
                "AccountInviteSuccess-route",
                "invited",
                new { controller = "Account", action = "InviteSuccess" });

            routes.MapRoute(
                "AccountActivate-route",
                "activate",
                new { controller = "Account", action = "Activate" });

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