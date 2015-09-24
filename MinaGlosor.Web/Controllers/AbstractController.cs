using System;
using System.Linq;
using System.Web.Mvc;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Controllers
{
    public class AbstractController : Controller
    {
        public IKernel Kernel { get; set; }

        public CommandExecutor CommandExecutor { get; set; }

        public QueryExecutor QueryExecutor { get; set; }

        // TODO Is this needed?
        protected User CurrentUser { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Response.IsRequestBeingRedirected) return;

            using (Kernel.BeginScope())
            {
                var documentSession = Kernel.Resolve<IDocumentSession>();
                var user = documentSession.Query<User, UserIndex>().SingleOrDefault(x => x.Email == User.Identity.Name);
                if (user != null)
                    CurrentUser = user;

                // make sure there's an admin user
                var config = documentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
                if (config == null)
                {
                    config = new WebsiteConfig();
                    documentSession.Store(config);
                    documentSession.SaveChanges();
                }

                if (config.AdminUsers.Any())
                    return;
            }

            // first launch
            Response.Redirect("~/welcome");
            Response.End();
        }

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");

            using (Kernel.BeginScope())
            {
                var result = QueryExecutor.ExecuteQuery(query, CurrentUser);
                return result;
            }
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            if (command == null) throw new ArgumentNullException("command");

            using (Kernel.BeginScope())
            {
                var result = CommandExecutor.ExecuteCommand(command, CurrentUser);
                return result;
            }
        }
    }
}