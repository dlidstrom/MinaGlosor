using System;
using System.Diagnostics;
using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using Newtonsoft.Json;
using Raven.Client;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public abstract class BackgroundTaskHandler<TTask>
    {
        public IKernel Kernel { get; set; }

        public abstract void Handle(TTask task);

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.Execute(GetDocumentSession());
        }

        protected void ExecuteCommand<TDependentTask>(ICommand command, TDependentTask causedByTask)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (causedByTask == null) throw new ArgumentNullException("causedByTask");
            using (new ModelContext(ModelContext.CorrelationId))
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new PrivateMembersContractResolver(),
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Formatting.Indented
                };
                var commandAsJson = JsonConvert.SerializeObject(command, settings);
                var documentSession = GetDocumentSession();
                var changeLogEntry = new ChangeLogEntry(
                    string.Empty,
                    string.Empty,
                    Trace.CorrelationManager.ActivityId,
                    command.GetType(),
                    commandAsJson);
                documentSession.Store(changeLogEntry);

                TracingLogger.Information(
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteDependentCommand_3001,
                    "{0} <- {1}: {2}",
                    command.GetType().Name,
                    causedByTask.GetType().Name,
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