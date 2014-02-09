using MinaGlosor.Web.Data.Models;
using Raven.Client;

namespace MinaGlosor.Web.Data.Commands
{
    public class CreateUserCommand : ICommand
    {
        private readonly string userEmail;
        private readonly string password;

        public CreateUserCommand(string userEmail, string password)
        {
            this.userEmail = userEmail;
            this.password = password;
        }

        public void Execute(IDocumentSession session)
        {
            var user = new User(string.Empty, string.Empty, userEmail, password);
            session.Store(user);
        }
    }
}