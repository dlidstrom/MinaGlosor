using System;
using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using Raven.Client;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public abstract class AbstractHandle<TEvent> : IHandle<TEvent>
    {
        public IKernel Kernel { get; set; }

        public abstract void Handle(TEvent ev);

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.Execute(GetDocumentSession());
        }

        protected void ExecuteCommand(ICommand command, ModelEvent causedByEvent)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (causedByEvent == null) throw new ArgumentNullException("causedByEvent");
            using (new ModelContext(ModelContext.CorrelationId, causedByEvent.EventId))
            {
                TracingLogger.Information(
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteDependenCommand_3001,
                    "{0} <- {1}",
                    command.GetType().Name,
                    causedByEvent.GetType().Name);
                command.Execute(GetDocumentSession());
            }
        }

        private IDocumentSession GetDocumentSession()
        {
            return Kernel.Resolve<IDocumentSession>();
        }
    }
}