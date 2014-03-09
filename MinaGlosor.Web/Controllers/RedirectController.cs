using System.Web.Mvc;

namespace MinaGlosor.Web.Controllers
{
    public class RedirectController : ControllerBase
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}