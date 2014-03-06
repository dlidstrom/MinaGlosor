using System.Data.Entity;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data
{
    public interface IDbContext
    {
        IDbSet<User> Users { get; set; }

        IDbSet<WordList> WordLists { get; set; }

        IDbSet<Word> Words { get; set; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}