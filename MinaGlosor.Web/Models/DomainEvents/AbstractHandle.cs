using System;
using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
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
                command.Execute(GetDocumentSession());
            }
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command, ModelEvent causedByEvent)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (causedByEvent == null) throw new ArgumentNullException("causedByEvent");
            using (new ModelContext(ModelContext.CorrelationId, causedByEvent.EventId))
            {
                return command.Execute(GetDocumentSession());
            }
        }

        private IDocumentSession GetDocumentSession()
        {
            return Kernel.Resolve<IDocumentSession>();
        }
    }
}