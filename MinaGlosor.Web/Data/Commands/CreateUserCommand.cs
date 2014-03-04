using System.Threading.Tasks;

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

        public Task ExecuteAsync(IDbContext context)
        {
            //var user = new User(string.Empty, string.Empty, userEmail, password);
            //session.Store(user);
            return null;
        }
    }
}