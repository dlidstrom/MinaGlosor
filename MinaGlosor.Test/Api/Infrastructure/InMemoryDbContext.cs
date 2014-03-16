using System.Data.Entity;
using System.Threading.Tasks;
using MinaGlosor.Web.Data;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Test.Api.Infrastructure
{
    public class InMemoryDbContext : IDbContext
    {
        public InMemoryDbContext()
        {
            Users = new InMemoryDbSet<User>();
            WordLists = new InMemoryDbSet<WordList>();
            Words = new InMemoryDbSet<Word>();
            CreateAccountRequests = new InMemoryDbSet<CreateAccountRequest>();
            PracticeSessions = new InMemoryDbSet<PracticeSession>();
        }

        public IDbSet<User> Users { get; private set; }

        public IDbSet<WordList> WordLists { get; private set; }

        public IDbSet<Word> Words { get; private set; }

        public IDbSet<CreateAccountRequest> CreateAccountRequests { get; private set; }

        public IDbSet<PracticeSession> PracticeSessions { get; private set; }

        public int SaveChanges()
        {
            return 0;
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult(0);
        }
    }
}