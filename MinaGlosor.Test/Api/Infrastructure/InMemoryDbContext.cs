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
        }

        public IDbSet<User> Users { get; set; }

        public IDbSet<WordList> WordLists { get; set; }

        public IDbSet<Word> Words { get; set; }

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