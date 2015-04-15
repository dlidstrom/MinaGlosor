using System.Web.Routing;
using MinaGlosor.Web;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
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
        public void Logon()
        {
            RouteTable.Routes.Maps("GET", "~/logon", new { controller = "AccountLogon", action = "Logon" });
        }

        [Test]
        public void Logoff()
        {
            RouteTable.Routes.Maps("GET", "~/logoff", new { controller = "AccountLogoff", action = "Logoff" });
        }

        [Test]
        public void AccountInvite()
        {
            RouteTable.Routes.Maps("GET", "~/invite", new { controller = "AccountInvite", action = "Invite" });
        }

        [Test]
        public void AccountInviteSuccess()
        {
            RouteTable.Routes.Maps("GET", "~/invited", new { controller = "AccountInvite", action = "InviteSuccess" });
        }

        [Test]
        public void ActivateAccount()
        {
            RouteTable.Routes.Maps("GET", "~/activate", new { controller = "AccountActivate", action = "Activate" });
        }

        [Test]
        public void SetPassword()
        {
            RouteTable.Routes.Maps("GET", "~/setpassword", new { controller = "AccountPassword", action = "Set" });
        }

        [Test]
        public void ResetPassword()
        {
            RouteTable.Routes.Maps("GET", "~/resetpassword", new { controller = "ResetPassword", action = "Reset" });
        }

        [Test]
        public void ResetPasswordSuccess()
        {
            RouteTable.Routes.Maps("GET", "~/resetpasswordrequested", new { controller = "ResetPassword", action = "Success" });
        }
    }
}