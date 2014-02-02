using System;
using System.Linq;
using System.Web.Mvc;
using MinaGlosor.Web.Data;
using MinaGlosor.Web.Infrastructure.Indexes;
using MinaGlosor.Web.Models;
using Raven.Client;

namespace MinaGlosor.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public IDocumentSession DocumentSession { get; set; }

        protected User CurrentUser { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Request.IsAuthenticated)
            {
                CurrentUser = DocumentSession.Query<User, User_ByEmail>()
                    .FirstOrDefault(x => x.Email == User.Identity.Name);
            }

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

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            command.Execute(DocumentSession);
        }
    }
}