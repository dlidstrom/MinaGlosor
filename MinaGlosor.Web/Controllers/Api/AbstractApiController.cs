using System;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Web.Http;
using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Attributes;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Indexes;
using Newtonsoft.Json;
using Raven.Client;

namespace MinaGlosor.Web.Controllers.Api
{
    [CheckAppVersion, SaveChanges]
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

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            var documentSession = GetDocumentSession();
            if (!query.CanExecute(documentSession, CurrentUser))
            {
                throw new SecurityException("Operation not allowed");
            }

            var result = query.Execute(documentSession);
            var whatChanged = documentSession.Advanced.WhatChanged();
            if (whatChanged.Any())
            {
                TracingLogger.Warning(
                    EventIds.Warning_Transient_4XXX.Web_ChangesFromQuery_4002,
                    "Change detected from query: {0}",
                    JsonConvert.SerializeObject(whatChanged, Formatting.Indented));
            }

            return result;
        }

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            var documentSession = GetDocumentSession();
            if (!command.CanExecute(documentSession, CurrentUser)) throw new SecurityException("Operation not allowed");

            DoExecuteCommand(documentSession, command, session =>
                {
                    command.Execute(session);
                    return false;
                });
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            if (command == null) throw new ArgumentNullException("command");
            var documentSession = GetDocumentSession();
            if (!command.CanExecute(documentSession, CurrentUser)) throw new SecurityException("Operation not allowed");

            return DoExecuteCommand(documentSession, command, command.Execute);
        }

        [DebuggerStepThrough]
        protected IDocumentSession GetDocumentSession()
        {
            return Kernel.Resolve<IDocumentSession>();
        }

        private TResult DoExecuteCommand<TResult, TCommand>(
            IDocumentSession documentSession,
            TCommand command,
            Func<IDocumentSession, TResult> func)
        {
            using (new ModelContext(Trace.CorrelationManager.ActivityId))
            {
                var settings = new JsonSerializerSettings
                    {
                        ContractResolver = new PrivateMembersContractResolver(),
                        TypeNameHandling = TypeNameHandling.All
                    };
                var commandAsJson = JsonConvert.SerializeObject(command, Formatting.Indented, settings);
                var changeLogEntry = new ChangeLogEntry(
                    CurrentUser.Id,
                    CurrentUser.Email,
                    Trace.CorrelationManager.ActivityId,
                    command.GetType(),
                    commandAsJson);
                documentSession.Store(changeLogEntry);

                TracingLogger.Information(
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteCommand_3000,
                    commandAsJson);
                return func.Invoke(documentSession);
            }
        }
    }
}