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

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            command.Execute(GetDocumentSession());
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            if (command == null) throw new ArgumentNullException("command");
            return command.Execute(GetDocumentSession());
        }

        private IDocumentSession GetDocumentSession()
        {
            return Kernel.Resolve<IDocumentSession>();
        }
    }
}