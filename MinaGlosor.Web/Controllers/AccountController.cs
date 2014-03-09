using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MinaGlosor.Web.Data.Commands;
using MinaGlosor.Web.Data.Queries;

namespace MinaGlosor.Web.Controllers
{
    public class AccountController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Invite(RequestCreateAccount request)
        {
            if (await ExecuteQueryAsync(new GetUserByEmailQuery(request.UserEmail)) != null)
                ModelState.AddModelError("Email", "E-postadressen finns redan");

            if (ModelState.IsValid == false)
                return RedirectToAction("Index", "Home");

            await ExecuteCommandAsync(new CreateAccountRequestCommand(request.UserEmail));

            return RedirectToAction("InviteSuccess");
        }

        public async Task<ActionResult> Activate(Guid activationCode)
        {
            var createAccountRequest = await ExecuteQueryAsync(new GetCreateAccountRequestQuery(activationCode));
            if (createAccountRequest == null)
                throw new HttpException(404, "Not found");

            if (createAccountRequest.HasBeenUsed())
                throw new HttpException(404, "Already used");

            return View(new ActivateAccountViewModel { ActivationCode = activationCode });
        }

        [HttpPost]
        public async Task<ActionResult> Activate(ActivateAccountViewModel vm)
        {
            if (ModelState.IsValid == false)
                return View(vm);

            var createAccountRequest = await ExecuteQueryAsync(new GetCreateAccountRequestQuery(vm.ActivationCode));
            if (createAccountRequest == null)
                throw new HttpException(404, "Not found");

            if (createAccountRequest.HasBeenUsed())
                throw new HttpException(404, "Already used");

            createAccountRequest.MarkAsUsed();
            await ExecuteCommandAsync(new CreateUserCommand(createAccountRequest.Email, vm.Password));
            FormsAuthentication.SetAuthCookie(createAccountRequest.Email, true);
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
        public async Task<ActionResult> Logon(LogonRequest request)
        {
            if (ModelState.IsValid == false)
                return View();
            var user = await ExecuteQueryAsync(new GetUserByEmailQuery(request.Email));
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
            public string UserEmail { get; set; }
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
            public Guid ActivationCode { get; set; }

            [Required]
            public string Password { get; set; }

            [Required, System.ComponentModel.DataAnnotations.Compare("Password")]
            public string PasswordConfirm { get; set; }
        }
    }
}