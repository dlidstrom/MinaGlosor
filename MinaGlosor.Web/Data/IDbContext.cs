using System.Data.Entity;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data
{
    public interface IDbContext
    {
        IDbSet<User> Users { get; }

        IDbSet<WordList> WordLists { get; }

        IDbSet<Word> Words { get; }

        IDbSet<CreateAccountRequest> CreateAccountRequests { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}