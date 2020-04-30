using System;
using System.Collections.Generic;
using System.Linq;
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
                        var indexOfSlash = username.IndexOf("/", StringComparison.Ordinal);
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
#pragma warning disable 162
                } while (false);
#pragma warning restore 162

                TracingLogger.Information("User {0}", username);
                var isAdmin = CurrentUser != null && CurrentUser.IsAdmin;
                var currentUserViewModel = new CurrentUserViewModel(isAdmin, CurrentUser != null ? CurrentUser.Username : string.Empty);
                return View("LoggedIn", currentUserViewModel);
            }

            return View("RegisterOrLogin", new RegisterViewModel(DateTime.Now.DayOfYear));
        }

        public class RegisterViewModel
        {
            private readonly Dictionary<int, string> toText =
                new Dictionary<int, string> {
                    { 0, "" },
                    { 1 , "ett" },
                    { 2, "två" },
                    { 3, "tre" },
                    { 4, "fyra" },
                    { 5, "fem" },
                    { 6, "sex" },
                    { 7, "sju" },
                    { 8, "åtta" },
                    { 9, "nio" },
                    { 10, "tio" },
                    { 11, "elva" },
                    { 12, "tolv" },
                    { 13, "tretton" },
                    { 14, "fjorton" },
                    { 15, "femton" },
                    { 16, "sexton" },
                    { 17, "sjutton" },
                    { 18, "arton" },
                    { 19, "nitton" },
                    { 20, "tjugo" },
                    { 30, "trettio" },
                    { 40, "fyrtio" },
                    { 50, "femtio" },
                    { 60, "sextio" },
                    { 70, "sjuttio" },
                    { 80, "åttio" },
                    { 90, "nittio" }
                };

            public RegisterViewModel(int secret)
            {
                var parts = new List<string>();
                if (secret >= 100) parts.Add($"{toText[secret / 100]}hundra");
                if (secret % 100 >= 20)
                {
                    parts.Add($"{toText[secret % 100 / 10 * 10]}");
                    parts.Add($"{toText[secret % 10]}");
                }
                else
                {
                    parts.Add($"{toText[secret % 100]}");
                }

                SecretAsText = string.Join("-", parts.Where(x => string.IsNullOrEmpty(x) == false));
            }

            public string SecretAsText { get; }
        }
    }
}