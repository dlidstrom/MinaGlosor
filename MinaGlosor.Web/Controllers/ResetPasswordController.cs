using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Web.Mvc;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers
{
    public class ResetPasswordController : AbstractController
    {
        public ActionResult Reset()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reset(ResetPasswordViewModel vm)
        {
            if (ModelState.IsValid == false)
                return View();

            var user = ExecuteQuery(new GetUserByEmailQuery(vm.Email));
            if (user == null)
                ModelState.AddModelError("Användarnamn", "Användaren finns inte");

            if (ModelState.IsValid == false)
                return View();

            Debug.Assert(user != null, "user != null");
            ExecuteCommand(new CreateResetPasswordRequestCommand(user.Id));

            return View();
        }

        public class ResetPasswordViewModel
        {
            [Required]
            public string Email { get; set; }
        }
    }
}