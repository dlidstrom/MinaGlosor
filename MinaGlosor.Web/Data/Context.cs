using System.Data.Entity;
using MinaGlosor.Web.Data.Migrations;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data
{
    public class Context : DbContext, IDbContext
    {
        public Context()
            : base("MainDb")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
        }

        public IDbSet<User> Users { get; set; }

        public IDbSet<WordList> WordLists { get; set; }

        public IDbSet<Word> Words { get; set; }

        public IDbSet<CreateAccountRequest> CreateAccountRequests { get; set; }

        public IDbSet<PracticeSession> PracticeSessions { get; set; }

        public IDbSet<WordScore> WordScores { get; set; }

        public IDbSet<PracticeWord> PracticeWords { get; set; }
    }
}