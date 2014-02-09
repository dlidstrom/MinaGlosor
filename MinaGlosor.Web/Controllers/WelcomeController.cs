using System.Web.Mvc;
using MinaGlosor.Web.Data.Commands;
using MinaGlosor.Web.Data.Queries;

namespace MinaGlosor.Web.Controllers
{
    public class WelcomeController : ControllerBase
    {
        public ActionResult Index()
        {
            if (ExecuteQuery(new GetUserQuery("Admin")) != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new InitialData("admin@" + Request.ServerVariables["HTTP_HOST"]));
        }

        [HttpPost]
        public ActionResult Create(CreateAdminUserRequest request)
        {
            if (ExecuteQuery(new GetUserQuery("Admin")) == null)
                ExecuteCommand(new CreateAdminUserCommand(request.UserEmail, request.Password));

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