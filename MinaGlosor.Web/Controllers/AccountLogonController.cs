using System.ComponentModel.DataAnnotations;
using System.Web;
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
            var user = ExecuteQuery(new GetUserByEmailQuery(request.Email));

            // TODO Fix this!
            if (user == null)
                throw new HttpException(404, "Not found");

            if (user.ValidatePassword(request.Password) == false)
                ModelState.AddModelError("Password", "Felaktigt lösenord");

            if (ModelState.IsValid == false)
                return View();

            FormsAuthentication.SetAuthCookie(user.Email, true);
            return RedirectToAction("Index", "Home");
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