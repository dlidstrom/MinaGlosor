using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.ProgressModel;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class CreateProgressCommandHandler : CommandHandlerBase<CreateProgressCommand, CreateProgressCommand.Result>
    {
        public override CreateProgressCommand.Result Handle(CreateProgressCommand command)
        {
            var progress = new Progress(
                command.OwnerId,
                command.WordListId);
            Session.Store(progress);
            return new CreateProgressCommand.Result(progress);
        }

        public override bool CanExecute(CreateProgressCommand command, User currentUser)
        {
            return true;
        }
    }
}