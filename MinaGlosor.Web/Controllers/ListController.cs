using System.Linq;
using System.Web.Mvc;
using MinaGlosor.Web.Data.Commands;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Infrastructure.Indexes;
using Raven.Client.Linq;

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

        [ChildActionOnly]
        public PartialViewResult Stored()
        {
            var items = DocumentSession.Query<WordList, WordListIndex>()
                           .Where(x => x.OwnerId == CurrentUser.Id)
                           .OrderBy(x => x.Name)
                           .ToArray();
            return PartialView(items);
        }
    }
}