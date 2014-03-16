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

        IDbSet<PracticeSession> PracticeSessions { get; }

        IDbSet<WordScore> WordScores { get; }

        IDbSet<PracticeWord> PracticeWords { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}