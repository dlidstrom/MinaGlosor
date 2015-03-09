using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Security;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers
{
    public class AccountLogonController : AbstractController
    {
        public ActionResult Logon()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logon(LogonRequest request)
        {
            if (ModelState.IsValid == false)
                return View();

            var user = ExecuteQuery(new GetUserByEmailQuery(request.Email)) ?? ExecuteQuery(new GetUserByUsernameQuery(request.Email));

            if (user == null)
                ModelState.AddModelError("Användarnamn", "Ingen användare återfanns");
            else if (user.ValidatePassword(request.Password) == false)
                ModelState.AddModelError("Lösenord", "Felaktigt lösenord");

            if (ModelState.IsValid == false)
                return View();

            Debug.Assert(user != null, "user != null");
            FormsAuthentication.SetAuthCookie(user.Email, true);
            return RedirectToAction("Index", "Home", new { username = user.Username });
        }

        public class LogonRequest
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }
        }
    }
}