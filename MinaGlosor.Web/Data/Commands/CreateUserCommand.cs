using System;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Commands
{
    public class CreateUserCommand : ICommand
    {
        private readonly string userEmail;
        private readonly string password;
        private readonly UserRole userRole;

        public CreateUserCommand(string userEmail, string password, UserRole userRole)
        {
            if (userEmail == null) throw new ArgumentNullException("userEmail");
            if (password == null) throw new ArgumentNullException("password");
            this.userEmail = userEmail;
            this.password = password;
            this.userRole = userRole;
        }

        public Task ExecuteAsync(IDbContext context)
        {
            context.Users.Add(new User(userEmail, password, userRole));
            return Task.FromResult(0);
        }
    }
}