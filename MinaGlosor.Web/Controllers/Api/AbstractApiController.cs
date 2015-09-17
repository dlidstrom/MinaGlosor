using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Attributes;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Controllers.Api
{
    [CheckAppVersion]
    public abstract class AbstractApiController : ApiController
    {
        private User currentUser;

        public IKernel Kernel { get; set; }

        public CommandExecutor CommandExecutor { get; set; }

        public QueryExecutor QueryExecutor { get; set; }

        protected User CurrentUser
        {
            get
            {
                if (currentUser != null) return currentUser;

                currentUser = GetDocumentSession().Query<User, UserIndex>()
                                                  .FirstOrDefault(x => x.Email == User.Identity.Name);

                return currentUser;
            }
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

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command, User runAs)
        {
            if (runAs == null) throw new ArgumentNullException("runAs");
            if (command == null) throw new ArgumentNullException("command");
            using (Kernel.BeginScope())
            {
                var result = CommandExecutor.ExecuteCommand(command, runAs);
                return result;
            }
        }

        [DebuggerStepThrough]
        protected IDocumentSession GetDocumentSession()
        {
            return Kernel.Resolve<IDocumentSession>();
        }
    }
}