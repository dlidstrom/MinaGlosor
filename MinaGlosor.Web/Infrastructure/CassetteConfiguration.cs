using Cassette;
using Cassette.Scripts;

namespace MinaGlosor.Web.Infrastructure
{
    /// <summary>
    /// Configures the Cassette asset bundles for the web application.
    /// </summary>
    public class CassetteBundleConfiguration : IConfiguration<BundleCollection>
    {
        public void Configure(BundleCollection bundles)
        {
            bundles.AddUrlWithAlias(
                "//ajax.aspnetcdn.com/ajax/bootstrap/3.0.3/css/bootstrap.min.css",
                "bootstrap");
            bundles.AddUrlWithAlias(
                "//ajax.googleapis.com/ajax/libs/angularjs/1.2.8/angular.min.js",
                "angularjs");

            bundles.AddPerSubDirectory<ScriptBundle>("Content/app");
        }
    }
}