using Newtonsoft.Json;

namespace MinaGlosor.Web.Infrastructure
{
    public static class ObjectExtensions
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public static string ToJson(this object ob, JsonSerializerSettings settings = null)
        {
            var json = JsonConvert.SerializeObject(ob, Formatting.Indented, settings ?? Settings);
            return json;
        }
    }
}