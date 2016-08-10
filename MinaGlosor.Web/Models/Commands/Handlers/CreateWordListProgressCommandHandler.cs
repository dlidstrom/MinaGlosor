using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class CreateWordListProgressCommandHandler : CommandHandlerBase<CreateWordListProgressCommand, CreateWordListProgressCommand.Result>
    {
        public override CreateWordListProgressCommand.Result Handle(CreateWordListProgressCommand command)
        {
            var wordListProgress = new WordListProgress.Model(
                command.OwnerId,
                command.WordListId);
            Session.Store(wordListProgress);
            return new CreateWordListProgressCommand.Result(wordListProgress);
        }

        public override bool CanExecute(CreateWordListProgressCommand command, User currentUser)
        {
            return true;
        }
    }
}