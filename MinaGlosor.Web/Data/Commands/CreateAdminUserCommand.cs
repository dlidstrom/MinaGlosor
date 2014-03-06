using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Commands
{
    public class CreateAdminUserCommand : ICommand
    {
        private readonly string userEmail;
        private readonly string password;

        public CreateAdminUserCommand(string userEmail, string password)
        {
            this.userEmail = userEmail;
            this.password = password;
        }

        public Task ExecuteAsync(IDbContext context)
        {
            context.Users.Add(new User(string.Empty, string.Empty, userEmail, password));
            return Task.FromResult(0);
        }
    }
}