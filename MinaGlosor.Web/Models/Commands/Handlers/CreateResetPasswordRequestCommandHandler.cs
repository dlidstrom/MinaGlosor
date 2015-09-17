using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class CreateResetPasswordRequestCommandHandler : CommandHandlerBase<CreateResetPasswordRequestCommand, string>
    {
        public override string Handle(CreateResetPasswordRequestCommand command)
        {
            var id = KeyGeneratorBase.Generate<ResetPasswordRequest>(Session);
            Session.Store(new ResetPasswordRequest(id, command.Email));
            return id;
        }

        public override bool CanExecute(CreateResetPasswordRequestCommand command, User currentUser)
        {
            return true;
        }
    }
}