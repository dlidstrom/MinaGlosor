using System;
using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models.BackgroundTasks;
using Raven.Client;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public abstract class AbstractHandle<TEvent> : IHandle<TEvent>
    {
        public IKernel Kernel { get; set; }

        public CommandExecutor CommandExecutor { get; set; }

        public abstract void Handle(TEvent ev);

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.Execute(Kernel.Resolve<IDocumentSession>());
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command, ModelEvent causedByEvent)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (causedByEvent == null) throw new ArgumentNullException("causedByEvent");

            using (new ModelContext(ModelContext.CorrelationId, causedByEvent.EventId))
            {
                var commandAsJson = command.ToJson();
                TracingLogger.Information(
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteDependentCommand_3001,
                    "{0} <- {1}: {2}",
                    command.GetType().Name,
                    causedByEvent.GetType().Name,
                    commandAsJson);
                var result = CommandExecutor.ExecuteCommand(command, null);
                return result;
            }
        }

        protected void SendTask<TBody>(TBody body, ModelEvent causedByEvent) where TBody : class
        {
            if (body == null) throw new ArgumentNullException("body");
            if (causedByEvent == null) throw new ArgumentNullException("causedByEvent");

            var bodyAsJson = body.ToJson();
            TracingLogger.Information(
                EventIds.Informational_ApplicationLog_3XXX.Web_SendTask_3009,
                "{0} <- {1}: {2}",
                body.GetType().Name,
                causedByEvent.GetType().Name,
                bodyAsJson);
            var session = Kernel.Resolve<IDocumentSession>();
            var task = BackgroundTask.Create(ModelContext.CorrelationId, causedByEvent.EventId, body);
            session.Store(task);
        }
    }
}