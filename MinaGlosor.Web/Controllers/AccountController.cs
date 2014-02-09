﻿using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MinaGlosor.Web.Data.Commands;
using MinaGlosor.Web.Data.Queries;

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
            if (ExecuteQuery(new GetUserByEmailQuery(request.Email)) != null)
                ModelState.AddModelError("Email", "E-postadressen finns redan");

            if (ExecuteQuery(new GetCreateAccountRequest(request.Email)) != null)
            {
                ModelState.AddModelError("Email", "Inbjudan är redan skickad");
            }

            if (ModelState.IsValid == false)
                return View();

            ExecuteCommand(new CreateAccountRequestCommand(request.Email));

            return RedirectToAction("InviteSuccess");
        }

        public ActionResult Activate(string activationCode)
        {
            var createAccountRequest = ExecuteQuery(new GetCreateAccountRequestQuery(activationCode));
            if (createAccountRequest == null)
                throw new HttpException(404, "Not found");

            if (createAccountRequest.HasBeenUsed())
                throw new HttpException(404, "Already used");

            return View(new ActivateAccountViewModel { ActivationCode = activationCode });
        }

        [HttpPost]
        public ActionResult Activate(ActivateAccountViewModel vm)
        {
            if (ModelState.IsValid == false)
                return View(vm);

            var createAccountRequest = ExecuteQuery(new GetCreateAccountRequestQuery(vm.ActivationCode));
            if (createAccountRequest == null)
                throw new HttpException(404, "Not found");

            if (createAccountRequest.HasBeenUsed())
                throw new HttpException(404, "Already used");

            createAccountRequest.MarkAsUsed();
            ExecuteCommand(new CreateUserCommand(createAccountRequest.Email, vm.Password));
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
        public ActionResult Logon(LogonRequest request)
        {
            var user = ExecuteQuery(new GetUserByEmailQuery(request.Email));
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
}