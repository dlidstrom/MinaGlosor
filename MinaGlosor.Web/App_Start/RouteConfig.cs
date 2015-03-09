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

            routes.MapRoute(
                "Invite-route",
                "invite",
                new { controller = "AccountInvite", action = "Invite" });

            routes.MapRoute(
                "Invited-route",
                "invited",
                new { controller = "AccountInvite", action = "InviteSuccess" });

            routes.MapRoute(
                "Activate-route",
                "activate",
                new { controller = "AccountActivate", action = "Activate" });

            routes.MapRoute(
                "Logon-route",
                "logon",
                new { controller = "AccountLogon", action = "Logon" });

            routes.MapRoute(
                "Logoff-route",
                "logoff",
                new { controller = "AccountLogoff", action = "Logoff" });

            routes.MapRoute(
                name: "Default",
                url: "{*username}",
                defaults: new { controller = "Home", action = "Index", username = UrlParameter.Optional });

            // for angular routing
            routes.MapRoute(
                name: "Catchall",
                url: "{*catchall}",
                defaults: new { controller = "Redirect", action = "Index" });
        }
    }
}