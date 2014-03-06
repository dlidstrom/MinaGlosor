using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MinaGlosor.Web.Data;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public IDbContext Context { get; set; }

        protected User CurrentUser { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Response.IsRequestBeingRedirected) return;

            if (Request.IsAuthenticated)
            {
                var user = Context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
                if (user != null)
                    CurrentUser = user;
            }

            // make sure there's an admin user
            if (Context.Users.SingleOrDefault(x => x.Role == UserRole.Admin) != null)
                return;

            // first launch
            Response.Redirect("/welcome");
            Response.End();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction || filterContext.Exception != null) return;

            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw;
            }
        }

        protected Task ExecuteCommandAsync(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            return command.ExecuteAsync(Context);
        }

        protected Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.ExecuteAsync(Context);
        }
    }
}