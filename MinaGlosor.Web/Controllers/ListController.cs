using System.Web.Mvc;
using MinaGlosor.Web.Data.Commands;

namespace MinaGlosor.Web.Controllers
{
    public class ListController : ControllerBase
    {
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string name)
        {
            ExecuteCommand(new CreateWordListCommand(name, CurrentUser));
            return RedirectToAction("Index", "Home");
        }
    }
}