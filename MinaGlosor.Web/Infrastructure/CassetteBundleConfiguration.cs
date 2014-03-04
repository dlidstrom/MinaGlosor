using Cassette;
using Cassette.BundleProcessing;
using Cassette.Scripts;
using Cassette.Stylesheets;

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
                "//ajax.googleapis.com/ajax/libs/angularjs/1.2.8/angular.min.js",
                "angularjs");
            bundles.AddUrlWithAlias(
                "//ajax.googleapis.com/ajax/libs/angularjs/1.2.8/angular-route.min.js",
                "angular-routejs");
            bundles.AddUrlWithAlias(
                "//ajax.googleapis.com/ajax/libs/angularjs/1.2.8/angular-animate.min.js",
                "angular-animate");
            bundles.AddUrlWithAlias(
                "//code.jquery.com/jquery-1.11.0.min.js",
                "jquery");

            bundles.AddPerSubDirectory<ScriptBundle>("Content/app");

            bundles.Add<StylesheetBundle>("Content/readable.min.css", bundle =>
            {
                var index = bundle.Pipeline.IndexOf<MinifyAssets>();
                if (index >= 0) bundle.Pipeline.RemoveAt(index);
            });

            bundles.Add<StylesheetBundle>("Content/toastr.css");
            bundles.Add<StylesheetBundle>("Content/main.less");
        }
    }
}