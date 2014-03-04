using MinaGlosor.Web.Data.Models;
using Raven.Client;

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

        public void Execute(IDbContext session)
        {
            session.Store(new WordList(name, owner));
        }
    }
}