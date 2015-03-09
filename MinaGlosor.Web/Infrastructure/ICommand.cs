using MinaGlosor.Web.Models;
using Raven.Client;

namespace MinaGlosor.Web.Infrastructure
{
    public interface ICommand
    {
        bool CanExecute(IDocumentSession session, User currentUser);

        void Execute(IDocumentSession session);
    }

    public interface ICommand<out TResult>
    {
        bool CanExecute(IDocumentSession session, User currentUser);

        TResult Execute(IDocumentSession session);
    }
}