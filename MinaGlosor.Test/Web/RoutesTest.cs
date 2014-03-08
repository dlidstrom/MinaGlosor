using System.Web.Routing;
using MinaGlosor.Web;
using NUnit.Framework;

namespace MinaGlosor.Test.Web
{
    [TestFixture]
    public class RoutesTest
    {
        [SetUp]
        public void SetUp()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        [Test]
        public void MapsLogin()
        {
            RouteTable.Routes.Maps("GET", "~/logon", new { controller = "Account", action = "Logon" });
        }
    }
}