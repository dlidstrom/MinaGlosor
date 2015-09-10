using System;
using System.Diagnostics;
using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using Raven.Client;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public abstract class BackgroundTaskHandler<TTask>
    {
        public IKernel Kernel { get; set; }

        public CommandExecutor CommandExecutor { get; set; }

        public abstract void Handle(TTask task);

        protected TResult ExecuteCommand<TResult, TDependentTask>(ICommand<TResult> command, TDependentTask causedByTask)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (causedByTask == null) throw new ArgumentNullException("causedByTask");

            var commandAsJson = command.ToJson();
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
            var result = CommandExecutor.ExecuteCommand(command, null);
            return result;
        }

        private IDocumentSession GetDocumentSession()
        {
            return Kernel.Resolve<IDocumentSession>();
        }
    }
}