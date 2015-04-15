using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers
{
    public class AccountPasswordController : AbstractController
    {
        public ActionResult Set(string activationCode)
        {
            var resetPasswordRequest = ExecuteQuery(new GetResetPasswordRequestQuery(activationCode));
            if (resetPasswordRequest == null)
                throw new HttpException(404, "Not found");

            if (resetPasswordRequest.HasBeenUsed())
                throw new HttpException(401, "Already used");

            return View(new SetPasswordViewModel { ActivationCode = activationCode });
        }

        [HttpPost]
        public ActionResult Set(string activationCode, SetPasswordViewModel vm)
        {
            if (ModelState.IsValid == false)
                return View(vm);

            var resetPasswordRequest = ExecuteQuery(new GetResetPasswordRequestQuery(activationCode));

            if (resetPasswordRequest.HasBeenUsed())
                throw new HttpException(401, "Already used");

            var user = ExecuteQuery(new GetUserByEmailQuery(resetPasswordRequest.Email));
            user.SetPassword(vm.Password);
            resetPasswordRequest.MarkAsUsed();

            FormsAuthentication.SetAuthCookie(user.Email, true);

            return RedirectToAction("Index", "Home");
        }

        public class SetPasswordViewModel
        {
            [HiddenInput]
            public string ActivationCode { get; set; }

            [Required]
            public string Password { get; set; }

            [Required, System.ComponentModel.DataAnnotations.Compare("Password")]
            public string PasswordConfirm { get; set; }
        }
    }
}