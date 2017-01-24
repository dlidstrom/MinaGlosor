using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Elmah.Contrib.WebApi;
using MinaGlosor.Web.Infrastructure.Attributes;
using MinaGlosor.Web.Infrastructure.Tracing;
using Newtonsoft.Json.Serialization;

// ReSharper disable CheckNamespace
namespace MinaGlosor.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.Filters.Add(new HttpRequestScopeAttribute());
            configuration.Filters.Add(new ElmahHandleErrorApiAttribute());
            configuration.Services.Add(typeof(IExceptionLogger), new TracingExceptionLogger());

            // camelCase by default
            var formatter = configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            formatter.Indent = true;

            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}