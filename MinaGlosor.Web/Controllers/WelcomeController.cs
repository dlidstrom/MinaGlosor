using System.Threading.Tasks;
using System.Web.Mvc;
using MinaGlosor.Web.Data.Commands;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Data.Queries;

namespace MinaGlosor.Web.Controllers
{
    public class WelcomeController : ControllerBase
    {
        public async Task<ActionResult> Index()
        {
            if (await ExecuteQueryAsync(new HasAdminUserQuery()))
                return RedirectToAction("Index", "Home");

            return View(new InitialData("admin@" + Request.ServerVariables["HTTP_HOST"]));
        }

        [HttpPost]
        public async Task<ActionResult> Index(CreateAdminUserRequest request)
        {
            if (await ExecuteQueryAsync(new HasAdminUserQuery()) == false)
                await ExecuteCommandAsync(new CreateUserCommand(request.UserEmail, request.Password, UserRole.Admin));

            return RedirectToAction("Index", "Home");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // don't load the non-existing admin user
        }

        public class InitialData
        {
            public InitialData(string suggestedEmail)
            {
                SuggestedEmail = suggestedEmail;
            }

            public string SuggestedEmail { get; set; }
        }

        public class CreateAdminUserRequest
        {
            public string UserEmail { get; set; }

            public string Password { get; set; }
        }
    }
}