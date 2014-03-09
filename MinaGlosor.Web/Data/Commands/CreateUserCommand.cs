using System;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Commands
{
    public class CreateUserCommand : ICommand
    {
        private readonly string userEmail;
        private readonly string password;

        public CreateUserCommand(string userEmail, string password)
        {
            if (userEmail == null) throw new ArgumentNullException("userEmail");
            if (password == null) throw new ArgumentNullException("password");
            this.userEmail = userEmail;
            this.password = password;
        }

        public Task ExecuteAsync(IDbContext context)
        {
            var user = new User(userEmail, password);
            context.Users.Add(user);
            return Task.FromResult(0);
        }
    }
}