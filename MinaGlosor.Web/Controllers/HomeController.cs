using System.Web.Mvc;

namespace MinaGlosor.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated == false)
                return View("RegisterOrLogin");

            return View();
        }
    }
}