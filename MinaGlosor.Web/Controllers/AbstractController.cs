using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Indexes;
using Newtonsoft.Json;
using Raven.Client;

namespace MinaGlosor.Web.Controllers
{
    public class AbstractController : Controller
    {
        public IKernel Kernel { get; set; }

        protected User CurrentUser { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Trace.CorrelationManager.ActivityId = SystemGuid.NewSequential;
            TracingLogger.Start(EventIds.Informational_Preliminary_1XXX.Web_Request_Executing_1001);
            if (Response.IsRequestBeingRedirected) return;

            var documentSession = GetDocumentSession();
            var user = documentSession.Query<User, UserIndex>().SingleOrDefault(x => x.Email == User.Identity.Name);
            if (user != null)
                CurrentUser = user;

            // make sure there's an admin user
            var firstAdminUser = documentSession.Query<User, UserIndex>()
                                                .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
                                                .FirstOrDefault(x => x.Role == UserRole.Admin);
            if (firstAdminUser != null)
                return;

            // first launch
            Response.Redirect("~/welcome");
            Response.End();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.IsChildAction && filterContext.Exception == null)
            {
                var documentSession = GetDocumentSession();
                var whatChanged = documentSession.Advanced.WhatChanged();
                if (whatChanged.Any())
                {
                    TracingLogger.Information("Saving {0} changes", whatChanged.Count);
                    documentSession.SaveChanges();
                }
            }

            TracingLogger.Stop(EventIds.Informational_Completion_2XXX.Web_Request_Executed_2001);
            Trace.CorrelationManager.ActivityId = default(Guid);
        }

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.Execute(GetDocumentSession());
        }

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");

            var documentSession = GetDocumentSession();
            if (documentSession.Advanced.WhatChanged().Any())
            {
                throw new ApplicationException("Detected changes, did you run more than one command?");
            }

            using (new ModelContext(Trace.CorrelationManager.ActivityId))
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new PrivateMembersContractResolver(),
                    TypeNameHandling = TypeNameHandling.All
                };
                var commandAsJson = JsonConvert.SerializeObject(command, Formatting.Indented, settings);
                string userId = null;
                string email = null;
                if (CurrentUser != null)
                {
                    userId = CurrentUser.Id;
                    email = CurrentUser.Email;
                }

                var changeLogEntry = new ChangeLogEntry(
                    userId ?? "<unknown user>",
                    email ?? "<unknown email>",
                    Trace.CorrelationManager.ActivityId,
                    command.GetType(),
                    commandAsJson);
                documentSession.Store(changeLogEntry);

                TracingLogger.Information(
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteCommand_3000,
                    commandAsJson);
                command.Execute(documentSession);
            }
        }

        private IDocumentSession GetDocumentSession()
        {
            return Kernel.Resolve<IDocumentSession>();
        }
    }
}