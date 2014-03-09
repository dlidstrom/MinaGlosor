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

        [TearDown]
        public void TearDown()
        {
            RouteTable.Routes.Clear();
        }

        [Test]
        public void Login()
        {
            RouteTable.Routes.Maps("GET", "~/logon", new { controller = "Account", action = "Logon" });
        }

        [Test]
        public void AccountInvite()
        {
            RouteTable.Routes.Maps("GET", "~/invite", new { controller = "Account", action = "Invite" });
        }

        [Test]
        public void AccountInviteSuccess()
        {
            RouteTable.Routes.Maps("GET", "~/invited", new { controller = "Account", action = "InviteSuccess" });
        }

        [Test]
        public void ActivateAccount()
        {
            RouteTable.Routes.Maps("GET", "~/activate", new { controller = "Account", action = "Activate" });
        }
    }
}