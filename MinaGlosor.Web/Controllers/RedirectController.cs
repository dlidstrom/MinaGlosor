using System.Web.Mvc;

namespace MinaGlosor.Web.Controllers
{
    public class RedirectController : AbstractController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}