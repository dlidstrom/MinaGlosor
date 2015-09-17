using System.Web.Http;
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
            configuration.Filters.Add(new HttpRequestScopeAttribute());
            configuration.Filters.Add(new ElmahHandleErrorApiAttribute());

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