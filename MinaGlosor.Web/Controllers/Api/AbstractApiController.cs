using System;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Web.Http;
using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Attributes;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.DomainEvents;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Controllers.Api
{
    [CheckAppVersion, SaveChanges, CorrelationId]
    public abstract class AbstractApiController : ApiController
    {
        private User currentUser;

        public IKernel Kernel { get; set; }

        public IDocumentStore DocumentStore { get; set; }

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

        protected Guid CorrelationId
        {
            get { return Trace.CorrelationManager.ActivityId; }
        }

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            var documentSession = GetDocumentSession();
            if (!query.CanExecute(documentSession, CurrentUser)) throw new SecurityException("Operation not allowed");
            return query.Execute(documentSession);
        }

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            var documentSession = GetDocumentSession();
            if (!command.CanExecute(documentSession, CurrentUser)) throw new SecurityException("Operation not allowed");

            using (DomainEvent.Correlate(SystemGuid.NewSequential))
            {
                command.Execute(documentSession);
            }
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            if (command == null) throw new ArgumentNullException("command");
            var documentSession = GetDocumentSession();
            if (!command.CanExecute(documentSession, CurrentUser)) throw new SecurityException("Operation not allowed");
            using (DomainEvent.Correlate(SystemGuid.NewSequential))
            {
                return command.Execute(documentSession);
            }
        }

        protected IDocumentSession GetDocumentSession()
        {
            return Kernel.Resolve<IDocumentSession>();
        }
    }
}