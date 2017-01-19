using MinaGlosor.Web.Models;
using Raven.Client;

namespace MinaGlosor.Web.Infrastructure
{
    public interface ICommandHandler<in TCommand, out TResult> where TCommand : ICommand<TResult>
    {
        IDocumentSession Session { get; set; }

        TResult Handle(TCommand command);

        bool CanExecute(TCommand command, User currentUser);
    }
}