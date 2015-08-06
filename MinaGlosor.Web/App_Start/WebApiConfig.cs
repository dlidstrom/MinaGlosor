using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Elmah.Contrib.WebApi;
using MinaGlosor.Web.Infrastructure.Attributes;
using Newtonsoft.Json.Serialization;

// ReSharper disable CheckNamespace
namespace MinaGlosor.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
            configuration.Filters.Add(new HttpRequestScopeAttribute());

            // camelCase by default
            var formatter = configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}