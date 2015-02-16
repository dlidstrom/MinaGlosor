using System.Web.Mvc;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.ViewModels;

namespace MinaGlosor.Web.Controllers
{
    public class HomeController : AbstractController
    {
        public ActionResult Index(string username)
        {
            if (Request.IsAuthenticated)
            {
                if (username == null || username != CurrentUser.Username)
                {
                    TracingLogger.Information("Username null, redirecting...");
                    return RedirectToAction("Index", new { username = CurrentUser.Username });
                }

                TracingLogger.Information("User {0}", username);
                var isAdmin = CurrentUser != null && CurrentUser.IsAdmin;
                var currentUserViewModel = new CurrentUserViewModel(isAdmin, CurrentUser != null ? CurrentUser.Username : string.Empty);
                return View("LoggedIn", currentUserViewModel);
            }

            return View("RegisterOrLogin");
        }
    }
}