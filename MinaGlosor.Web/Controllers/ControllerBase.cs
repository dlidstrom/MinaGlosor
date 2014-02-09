using System;
using System.Linq;
using System.Web.Mvc;
using Castle.MicroKernel;
using MinaGlosor.Web.Data;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Infrastructure.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public IKernel Kernel { get; set; }

        protected User CurrentUser { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Response.IsRequestBeingRedirected) return;

            if (Request.IsAuthenticated)
            {
                var user = GetDocumentSession().Query<User, User_ByEmail>()
                                               .FirstOrDefault(x => x.Email == User.Identity.Name);
                if (user != null)
                    CurrentUser = user;
            }

            // make sure there's an admin user
            if (GetDocumentSession().Load<User>("Admin") != null) return;

            // first launch
            Response.Redirect("/welcome");
            Response.End();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction || filterContext.Exception != null) return;

            GetDocumentSession().SaveChanges();
        }

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            command.Execute(GetDocumentSession());
        }

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.Execute(GetDocumentSession());
        }

        private IDocumentSession GetDocumentSession()
        {
            return Kernel.Resolve<IDocumentSession>();
        }
    }
}