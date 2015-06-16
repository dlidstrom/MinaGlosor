using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public abstract class AdminCommand<TResult> : ICommand<TResult>
    {
        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return currentUser.IsAdmin;
        }

        public abstract TResult Execute(IDocumentSession session);
    }
}