using System.Web.Mvc;
using MinaGlosor.Web.ViewModels;

namespace MinaGlosor.Web.Controllers
{
    public class HomeController : AbstractController
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
                return View("LoggedIn", new CurrentUserViewModel(CurrentUser != null && CurrentUser.IsAdmin));

            return View("RegisterOrLogin");
        }
    }
}