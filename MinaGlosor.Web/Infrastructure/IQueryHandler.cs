using MinaGlosor.Web.Models;

namespace MinaGlosor.Web.Infrastructure
{
    public interface IQueryHandler
    {
        bool CanExecute(User currentUser);
    }
}