using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class CreateWordListProgressCommandHandler : CommandHandlerBase<CreateWordListProgressCommand, CreateWordListProgressCommand.Result>
    {
        public override CreateWordListProgressCommand.Result Handle(CreateWordListProgressCommand command)
        {
            var progress = new Progress(
                command.OwnerId,
                command.WordListId);
            Session.Store(progress);
            return new CreateWordListProgressCommand.Result(progress);
        }

        public override bool CanExecute(CreateWordListProgressCommand command, User currentUser)
        {
            return true;
        }
    }
}