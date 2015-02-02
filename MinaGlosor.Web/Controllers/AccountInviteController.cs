using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Web.Mvc;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers
{
    public class AccountInviteController : AbstractController
    {
        [HttpPost]
        public ActionResult Index(RequestCreateAccount request)
        {
            if (request == null)
            {
                ModelState.AddModelError("Request", "Invalid request");
            }
            else if (ExecuteQuery(new GetUserByEmailQuery(request.UserEmail)) != null)
            {
                ModelState.AddModelError("Email", "E-postadressen finns redan");
            }
            else if (ExecuteQuery(new GetUserByUsernameQuery(request.Username)) != null)
            {
                ModelState.AddModelError("Email", "E-postadressen finns redan");
            }

            if (ModelState.IsValid == false)
                return RedirectToAction("Index", "Home");

            Debug.Assert(request != null, "request != null");
            ExecuteCommand(new CreateAccountRequestCommand(request.UserEmail));

            return RedirectToAction("InviteSuccess");
        }

        public ActionResult InviteSuccess()
        {
            return View();
        }

        public class RequestCreateAccount
        {
            [Required, MaxLength(254)]
            public string UserEmail { get; set; }

            [Required, MaxLength(20)]
            public string Username { get; set; }
        }
    }
}