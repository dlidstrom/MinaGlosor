using System.Threading.Tasks;

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

        public Task ExecuteAsync(IDbContext session)
        {
            //var user = new User(string.Empty, string.Empty, userEmail, password);
            //user.SetRole(UserRole.Admin);
            //session.Store(user);
            return null;
        }
    }
}