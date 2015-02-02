using System.Web.Http;
using Newtonsoft.Json.Serialization;

// ReSharper disable CheckNamespace
namespace MinaGlosor.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            //configuration.Services.Add(typeof(IExceptionLogger), new CustomExceptionLogger());

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