using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MinaGlosor.Web.App_Start;

namespace MinaGlosor.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
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
    }
}