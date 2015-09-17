using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class CreateAccountRequestCommandHandler : CommandHandlerBase<CreateAccountRequestCommand, string>
    {
        public override string Handle(CreateAccountRequestCommand command)
        {
            var request = new CreateAccountRequest(
                KeyGeneratorBase.Generate<CreateAccountRequest>(Session),
                command.Email);
            Session.Store(request);

            return request.Id;
        }

        public override bool CanExecute(CreateAccountRequestCommand command, User currentUser)
        {
            return true;
        }
    }
}