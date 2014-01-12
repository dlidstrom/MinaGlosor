using System.Web.Mvc;

namespace MinaGlosor.Web.Controllers
{
    public class WelcomeController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // don't load the non-existing admin user
        }
    }
}