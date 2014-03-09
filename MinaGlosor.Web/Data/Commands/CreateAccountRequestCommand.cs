using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

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
            var accountRequest = new CreateAccountRequest(email);
            context.CreateAccountRequests.Add(accountRequest);
            return Task.FromResult(0);
        }
    }
}