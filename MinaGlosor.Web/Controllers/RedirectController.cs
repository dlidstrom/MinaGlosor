using System.Web.Mvc;

namespace MinaGlosor.Web.Controllers
{
    public class RedirectController : AbstractController
    {
        public ActionResult Index()
        {
            //if (Request.IsAuthenticated)
            //{
            //    var isAdmin = CurrentUser != null && CurrentUser.IsAdmin;
            //    var currentUserViewModel = new CurrentUserViewModel(isAdmin, CurrentUser != null ? CurrentUser.Username : string.Empty);
            //    return View("LoggedIn", currentUserViewModel);
            //}

            return RedirectToAction("Index", "Home");
        }
    }
}