using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateUserCommand : ICommand
    {
        private readonly string email;
        private readonly string password;
        private readonly UserRole userRole;
        private readonly string username;

        public CreateUserCommand(string email, string password, string username, UserRole userRole)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (password == null) throw new ArgumentNullException("password");
            if (username == null) throw new ArgumentNullException("username");

            this.email = email;
            this.password = password;
            this.userRole = userRole;
            this.username = username;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public void Execute(IDocumentSession session)
        {
            // check for existing user
            var existing = session.Query<User, UserIndex>().SingleOrDefault(x => x.Username == username);
            if (existing != null)
            {
                var message = string.Format("Username {0} already registered", username);
                throw new ApplicationException(message);
            }

            session.Store(new User(email, password, username, userRole));
        }
    }
}