using System.Web.Mvc;

namespace MinaGlosor.Web.Controllers
{
    public class RedirectController : ControllerBase
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
                return View("LoggedIn");

            return RedirectToAction("Index", "Home");
        }
    }
}