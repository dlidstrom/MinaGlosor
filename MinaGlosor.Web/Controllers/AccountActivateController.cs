using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers
{
    public class AccountPassword : AbstractController
    {
        public ActionResult Set(Guid activationCode)
        {
            var resetPasswordRequest = ExecuteQuery(new GetResetPasswordRequest(activationCode));
            if (resetPasswordRequest == null)
                throw new HttpException(404, "Not found");

            if (resetPasswordRequest.HasBeenUser())
                throw new HttpException(401, "Already used");

            return View(new ResetPasswordViewModel { ActivationCode = activationCode });
        }
    }

    public class AccountActivateController : AbstractController
    {
        public ActionResult Activate(Guid activationCode)
        {
            var createAccountRequest = ExecuteQuery(new GetCreateAccountRequestQuery(activationCode));
            if (createAccountRequest == null)
                throw new HttpException(404, "Not found");

            if (createAccountRequest.HasBeenUsed())
                throw new HttpException(401, "Already used");

            return View(new ActivateAccountViewModel { ActivationCode = activationCode });
        }

        [HttpPost]
        public ActionResult Activate(ActivateAccountViewModel vm)
        {
            var createAccountRequest = ExecuteQuery(new GetCreateAccountRequestQuery(vm.ActivationCode));
            if (createAccountRequest == null)
                throw new HttpException(404, "Not found");

            if (createAccountRequest.HasBeenUsed())
                throw new HttpException(404, "Already used");

            if (ExecuteQuery(new GetUserByUsernameQuery(vm.Username)) != null)
                ModelState.AddModelError("Användarnamn", "Användarnamnet existerar redan");

            if (ModelState.IsValid == false)
                return View(vm);

            createAccountRequest.MarkAsUsed();
            ExecuteCommand(new CreateUserCommand(createAccountRequest.Email, vm.Password, vm.Username, UserRole.Basic));
            FormsAuthentication.SetAuthCookie(createAccountRequest.Email, true);
            return RedirectToAction("Index", "Home");
        }

        public class ActivateAccountViewModel
        {
            [HiddenInput]
            public Guid ActivationCode { get; set; }

            [Required, RegularExpression(Models.User.UsernamePattern)]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }

            [Required, System.ComponentModel.DataAnnotations.Compare("Password")]
            public string PasswordConfirm { get; set; }
        }
    }
}