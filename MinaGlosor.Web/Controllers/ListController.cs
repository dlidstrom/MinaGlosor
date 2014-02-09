using System.Web.Mvc;
using MinaGlosor.Web.Data.Commands;
using MinaGlosor.Web.Data.Queries;

namespace MinaGlosor.Web.Controllers
{
    [Authorize]
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

        [ChildActionOnly]
        public PartialViewResult Stored()
        {
            var results = ExecuteQuery(new GetWordListsQuery(CurrentUser));
            return PartialView(results);
        }
    }
}