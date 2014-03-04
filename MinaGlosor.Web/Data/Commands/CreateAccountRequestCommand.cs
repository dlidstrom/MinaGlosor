using MinaGlosor.Web.Data.Models;
using Raven.Client;

namespace MinaGlosor.Web.Data.Commands
{
    public class CreateAccountRequestCommand : ICommand
    {
        private readonly string email;

        public CreateAccountRequestCommand(string email)
        {
            this.email = email;
        }

        public void Execute(IDbContext session)
        {
            var accountRequest = new CreateAccountRequest(email);
            session.Store(accountRequest);
        }
    }
}