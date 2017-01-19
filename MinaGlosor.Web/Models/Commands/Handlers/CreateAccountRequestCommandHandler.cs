using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class CreateAccountRequestCommandHandler : ICommandHandler<CreateAccountRequestCommand, string>
    {
        public IDocumentSession Session { get; set; }

        public string Handle(CreateAccountRequestCommand command)
        {
            var request = new CreateAccountRequest(
                KeyGeneratorBase.Generate<CreateAccountRequest>(Session),
                command.Email);
            Session.Store(request);

            return request.Id;
        }

        public bool CanExecute(CreateAccountRequestCommand command, User currentUser)
        {
            return true;
        }
    }
}