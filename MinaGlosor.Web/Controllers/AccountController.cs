using System.Linq;
using System.Web.Mvc;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Indexes;
using MinaGlosor.Web.Models;

namespace MinaGlosor.Web.Controllers
{
    public class AccountController : ControllerBase
    {
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Request request)
        {
            if (DocumentSession.Query<User, User_ByEmail>().FirstOrDefault(x => x.Email == request.Email) != null)
                ModelState.AddModelError("Email", "E-postadressen finns redan");

            if (DocumentSession.Query<CreateAccountRequest>().FirstOrDefault(x => x.Email == request.Email) != null)
                ModelState.AddModelError("Email", "Inbjudan är redan skickad");

            if (ModelState.IsValid == false)
                return View();

            var accountRequest = new CreateAccountRequest(request.Email);
            DocumentSession.Store(accountRequest);

            Emails.InviteUser(accountRequest);
            return RedirectToAction("Index", "Home");
        }

        public class Request
        {
            public string Email { get; set; }
        }
    }
}