using System.Web.Mvc;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Controllers
{
    public class WelcomeController : AbstractController
    {
        public ActionResult Index()
        {
            if (ExecuteQuery(new HasAdminUserQuery()))
                return RedirectToAction("Index", "Home");

            return View(new InitialData("admin@" + Request.ServerVariables["HTTP_HOST"]));
        }

        [HttpPost]
        public ActionResult Index(CreateAdminUserRequest request)
        {
            if (ExecuteQuery(new HasAdminUserQuery()) == false)
                ExecuteCommand(new CreateUserCommand(request.UserEmail, request.Password, request.Username, UserRole.Admin));

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

            public string Username { get; set; }
        }
    }
}