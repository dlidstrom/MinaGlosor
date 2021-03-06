using MinaGlosor.Web.Models;
using Raven.Client;

namespace MinaGlosor.Web.Infrastructure
{
    public abstract class CommandHandlerBase<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        public IDocumentSession Session { get; set; }

        public abstract TResult Handle(TCommand command);

        public abstract bool CanExecute(TCommand command, User currentUser);
    }
}