using System;
using System.Web.Mvc;
using Castle.MicroKernel;
using MinaGlosor.Web.Data;
using MinaGlosor.Web.Data.Models;
using Raven.Client;

namespace MinaGlosor.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public IKernel Kernel { get; set; }

        protected User CurrentUser { get; private set; }

        protected override async void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Response.IsRequestBeingRedirected) return;

            if (Request.IsAuthenticated)
            {
                var user = await GetDocumentSession().Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
                if (user != null)
                    CurrentUser = user;
            }

            // make sure there's an admin user
            if (await GetDocumentSession().Users.SingleOrDefaultAsync(x => x.Role == UserRole.Admin) != null)
                return;

            // first launch
            Response.Redirect("/welcome");
            Response.End();
        }

        protected override async void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction || filterContext.Exception != null) return;

            await GetDocumentSession().SaveChangesAsync();
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

        private IDbContext GetDocumentSession()
        {
            return Kernel.Resolve<IDbContext>();
        }
    }
}