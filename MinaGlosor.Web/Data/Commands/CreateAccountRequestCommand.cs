using System.Threading.Tasks;

namespace MinaGlosor.Web.Data.Commands
{
    public class CreateAccountRequestCommand : ICommand
    {
        private readonly string email;

        public CreateAccountRequestCommand(string email)
        {
            this.email = email;
        }

        public Task ExecuteAsync(IDbContext context)
        {
            //var accountRequest = new CreateAccountRequest(email);
            //session.Store(accountRequest);
            return Task.FromResult(0);
        }
    }
}