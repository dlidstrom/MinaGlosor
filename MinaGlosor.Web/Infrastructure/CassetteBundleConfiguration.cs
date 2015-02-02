using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace MinaGlosor.Web.Infrastructure
{
    public class CassetteBundleConfiguration : IConfiguration<BundleCollection>
    {
        public void Configure(BundleCollection bundles)
        {
            bundles.Add<ScriptBundle>("wwwroot/app");
            bundles.Add<StylesheetBundle>("wwwroot/main.less");
        }
    }
}