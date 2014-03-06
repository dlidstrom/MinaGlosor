using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Commands
{
    public class CreateWordListCommand : ICommand
    {
        private readonly string name;
        private readonly User owner;

        public CreateWordListCommand(string name, User owner)
        {
            this.name = name;
            this.owner = owner;
        }

        public Task ExecuteAsync(IDbContext context)
        {
            context.WordLists.Add(new WordList(name, owner));
            return Task.FromResult(0);
        }
    }
}