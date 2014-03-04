using System.Data.Entity;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data
{
    public class Context : DbContext, IDbContext
    {
        public Context()
            : base("MainDb")
        {
        }

        public IDbSet<User> Users { get; set; }

        public IDbSet<WordList> WordLists { get; set; }

        public IDbSet<Word> Words { get; set; }
    }
}