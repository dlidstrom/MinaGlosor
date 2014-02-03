using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Indexes;
using MinaGlosor.Web.Models;

namespace MinaGlosor.Web.Controllers
{
    public class AccountController : ControllerBase
    {
        public ActionResult Invite()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Invite(RequestCreateAccount request)
        {
            if (DocumentSession.Query<User, User_ByEmail>().FirstOrDefault(x => x.Email == request.Email) != null)
                ModelState.AddModelError("Email", "E-postadressen finns redan");

            if (DocumentSession.Query<CreateAccountRequest, CreateAccountRequestIndex>()
                               .FirstOrDefault(x => x.Email == request.Email) != null)
            {
                ModelState.AddModelError("Email", "Inbjudan är redan skickad");
            }

            if (ModelState.IsValid == false)
                return View();

            var accountRequest = new CreateAccountRequest(request.Email);
            DocumentSession.Store(accountRequest);

            Emails.InviteUser(accountRequest);
            return RedirectToAction("InviteSuccess");
        }

        public ActionResult Activate(string activationCode)
        {
            return View(new ActivateAccountViewModel { ActivationCode = activationCode });
        }

        [HttpPost]
        public ActionResult Activate(ActivateAccountViewModel vm)
        {
            if (ModelState.IsValid == false)
                return View(vm);

            var createAccountRequest = DocumentSession.Query<CreateAccountRequest, CreateAccountRequestIndex>()
                                                      .FirstOrDefault(x => x.ActivationCode == vm.ActivationCode);
            if (createAccountRequest == null)
                throw new HttpException(404, "Not found");

            var user = new User("", "", createAccountRequest.Email, vm.Password);
            DocumentSession.Store(user);
            FormsAuthentication.SetAuthCookie(user.Email, true);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult InviteSuccess()
        {
            return View();
        }

        public ActionResult Logon()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logon(LogonRequest request)
        {
            var user = DocumentSession.Query<User, User_ByEmail>().FirstOrDefault(x => x.Email == request.Email);
            if (user == null)
                throw new HttpException(404, "Not found");

            if (user.ValidatePassword(request.Password) == false)
                ModelState.AddModelError("Password", "Felaktigt lösenord");

            if (ModelState.IsValid == false)
                return View();

            FormsAuthentication.SetAuthCookie(user.Email, true);
            return RedirectToAction("Index", "Home");
        }

        public class RequestCreateAccount
        {
            public string Email { get; set; }
        }
    }

    public class LogonRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class ActivateAccountViewModel
    {
        [HiddenInput]
        public string ActivationCode { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}