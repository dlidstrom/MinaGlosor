using System.IO;
using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace MinaGlosor.Web.Infrastructure
{
    public class CassetteBundleConfiguration : IConfiguration<BundleCollection>
    {
        public void Configure(BundleCollection bundles)
        {
            bundles.Add<ScriptBundle>("wwwroot/app", new FileSearch { SearchOption = SearchOption.AllDirectories });
            bundles.Add<ScriptBundle>("wwwroot/external/head_js");
            bundles.Add<ScriptBundle>("wwwroot/external/footer_js");
            bundles.Add<StylesheetBundle>("wwwroot/");
            bundles.Add<StylesheetBundle>("wwwroot/app", new FileSearch { SearchOption = SearchOption.AllDirectories });
        }
    }
}