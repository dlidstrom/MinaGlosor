using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class CreateUserCommandHandler : CommandHandlerBase<CreateUserCommand, string>
    {
        public override string Handle(CreateUserCommand command)
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
            return id;
        }

        public override bool CanExecute(CreateUserCommand command, User currentUser)
        {
            return true;
        }
    }
}