using System.Web.Mvc;
using MinaGlosor.Web.Models;
using Raven.Client;

namespace MinaGlosor.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public IDocumentSession DocumentSession { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // make sure there's an admin user
            if (DocumentSession.Load<User>("Admin") != null) return;

            // first launch
            Response.Redirect("/welcome");
            Response.End();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction || filterContext.Exception != null) return;

            DocumentSession.SaveChanges();
        }
    }
}