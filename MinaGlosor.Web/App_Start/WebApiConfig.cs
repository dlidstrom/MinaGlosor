using System.Web.Http;
using MinaGlosor.Web.Infrastructure;
using Newtonsoft.Json.Serialization;

// ReSharper disable CheckNamespace
namespace MinaGlosor.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.Filters.Add(new SaveChangesAttribute());

            // camelCase by default
            var formatter = config.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}