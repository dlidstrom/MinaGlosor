using System.Web.Mvc;
using MinaGlosor.Web.Models;

namespace MinaGlosor.Web.Controllers
{
    public class WelcomeController : ControllerBase
    {
        public ActionResult Index()
        {
            if (DocumentSession.Load<User>("Admin") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new InitialData("admin@" + Request.ServerVariables["HTTP_HOST"]));
        }

        [HttpPost]
        public ActionResult Create(CreateAdminUserRequest request)
        {
            if (DocumentSession.Load<User>("Admin") == null)
            {
                var user = new User(string.Empty, string.Empty, request.UserEmail, request.Password)
                    {
                        Id = "Admin"
                    };
                DocumentSession.Store(user);
            }

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