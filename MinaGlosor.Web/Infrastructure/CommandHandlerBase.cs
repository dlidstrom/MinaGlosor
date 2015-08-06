using MinaGlosor.Web.Models;
using Raven.Client;

namespace MinaGlosor.Web.Infrastructure
{
    public interface

    public abstract class CommandHandlerBase<TCommand> where TCommand : ICommand

    {
        public IDocumentSession Session { get; set; }

        public abstract void CanExecute(User currentUser);

        public abstract void Handle(TCommand command);
    }

    public abstract class CommandHandlerBase<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        public IDocumentSession Session { get; set; }

        public abstract void CanExecute(User currentUser);

        public abstract TResult Handle(TCommand command);
    }
}