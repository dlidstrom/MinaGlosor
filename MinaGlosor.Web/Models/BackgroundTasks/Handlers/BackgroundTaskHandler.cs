using System;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public abstract class BackgroundTaskHandler<TTask>
    {
        public CommandExecutor CommandExecutor { get; set; }

        public abstract void Handle(TTask task);

        protected void ExecuteCommand<TResult, TDependentTask>(ICommand<TResult> command, TDependentTask causedByTask)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (causedByTask == null) throw new ArgumentNullException("causedByTask");

            TracingLogger.Information(
                EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteDependentCommand_3001,
                "{0} <- {1}: {2}",
                command.GetType().Name,
                causedByTask.GetType().Name,
                command.ToJson());
            CommandExecutor.ExecuteCommand(command, null);
        }
    }
}