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
                do
                {
                    if (username == null)
                    {
                        TracingLogger.Information("Username null, redirecting...");
                    }
                    else if (username == CurrentUser.Username)
                    {
                        break;
                    }
                    else
                    {
                        var indexOfSlash = username.IndexOf("/", System.StringComparison.Ordinal);
                        if (indexOfSlash <= 0)
                        {
                            TracingLogger.Information("Username not found in URL (found '{0}'), redirecting...", username);
                        }
                        else
                        {
                            var usernameSubstring = username.Substring(0, indexOfSlash);
                            if (usernameSubstring == CurrentUser.Username)
                            {
                                break;
                            }

                            TracingLogger.Information("Username not found in URL (found '{0}'), redirecting...", usernameSubstring);
                        }
                    }

                    return RedirectToAction("Index", new { username = CurrentUser.Username });
                } while (false);

                TracingLogger.Information("User {0}", username);
                var isAdmin = CurrentUser != null && CurrentUser.IsAdmin;
                var currentUserViewModel = new CurrentUserViewModel(isAdmin, CurrentUser != null ? CurrentUser.Username : string.Empty);
                return View("LoggedIn", currentUserViewModel);
            }

            return View("RegisterOrLogin");
        }
    }
}