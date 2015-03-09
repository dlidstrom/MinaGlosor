using MinaGlosor.Web.Models;
using Raven.Client;

namespace MinaGlosor.Web.Infrastructure
{
    public interface IQuery<out TResult>
    {
        // TODO Check for null on current user!
        bool CanExecute(IDocumentSession session, User currentUser);

        TResult Execute(IDocumentSession session);
    }
}