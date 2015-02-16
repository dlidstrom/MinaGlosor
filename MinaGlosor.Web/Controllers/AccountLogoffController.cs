using System.Web.Mvc;
using System.Web.Security;

namespace MinaGlosor.Web.Controllers
{
    public class AccountLogoffController : AbstractController
    {
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}