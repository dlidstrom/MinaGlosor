using MinaGlosor.Web.Models;

namespace MinaGlosor.Web.Infrastructure
{
    public interface ICommandHandler
    {
        bool CanExecute(User currentUser);
    }
}