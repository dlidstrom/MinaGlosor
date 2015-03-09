using System.Security.Authentication;
using System.Web.Mvc;

namespace MinaGlosor.Web.Controllers
{
    public class AdminController : AbstractController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Response.IsRequestBeingRedirected) return;

            if (Request.IsAuthenticated == false || CurrentUser == null || CurrentUser.IsAdmin == false)
                throw new AuthenticationException("Not authorized");
        }
    }
}