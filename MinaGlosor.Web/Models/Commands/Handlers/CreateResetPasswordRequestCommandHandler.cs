using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class CreateResetPasswordRequestCommandHandler : ICommandHandler<CreateResetPasswordRequestCommand, string>
    {
        public IDocumentSession Session { get; set; }

        public string Handle(CreateResetPasswordRequestCommand command)
        {
            var id = KeyGeneratorBase.Generate<ResetPasswordRequest>(Session);
            Session.Store(new ResetPasswordRequest(id, command.Email));
            return id;
        }

        public bool CanExecute(CreateResetPasswordRequestCommand command, User currentUser)
        {
            return true;
        }
    }
}