using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class UserCommandHandler : ICommandHandler<CreateUserCommand, string>
    {
        public IDocumentSession Session { get; set; }

        public string Handle(CreateUserCommand command)
        {
            // check for existing user
            var existing = Session.Query<User, UserIndex>().SingleOrDefault(x => x.Username == command.Username);
            if (existing != null)
            {
                var message = string.Format("Username {0} already registered", command.Username);
                throw new ApplicationException(message);
            }

            var id = KeyGeneratorBase.Generate<User>(Session);
            Session.Store(new User(id, command.Email, command.Password, command.Username, command.UserRole));
            if (command.UserRole == UserRole.Admin)
            {
                var config = Session.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
                config.AddAdminUser(id);
            }

            return id;
        }

        public bool CanExecute(CreateUserCommand command, User currentUser)
        {
            return true;
        }
    }
}