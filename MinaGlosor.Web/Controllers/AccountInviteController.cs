using System;
using System.Collections.Generic;
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
        public ActionResult Invite(RequestCreateAccount request)
        {
            if (request == null)
            {
                ModelState.AddModelError("Request", "Invalid request");
            }
            else if (string.IsNullOrWhiteSpace(request.UserEmail) == false)
            {
                // try to catch the spammers
                return RedirectToAction("Index", "Home");
            }
            else if (ExecuteQuery(new GetUserByEmailQuery(request.UserEmail2)) != null)
            {
                ModelState.AddModelError("Email", "E-postadressen finns redan");
            }

            if (ModelState.IsValid == false)
                return RedirectToAction("Index", "Home");

            Debug.Assert(request != null, "request != null");
            ExecuteCommand(new CreateAccountRequestCommand(request.UserEmail2));

            return RedirectToAction("InviteSuccess");
        }

        public ActionResult InviteSuccess()
        {
            return View();
        }

        public class RequestCreateAccount : IValidatableObject
        {
            public string UserEmail { get; set; }

            [Required, MaxLength(254)]
            public string UserEmail2 { get; set; }

            [Required(ErrorMessage = "Du måste skriva talet som anges.")]
            public int? Secret { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (Secret.GetValueOrDefault() != DateTime.Now.Date.DayOfYear) yield return new ValidationResult("Du måste skriva talet som anges.");
            }
        }
    }
}
