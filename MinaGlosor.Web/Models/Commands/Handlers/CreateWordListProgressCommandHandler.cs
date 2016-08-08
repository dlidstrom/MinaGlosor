using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class CreateWordListProgressCommandHandler : CommandHandlerBase<CreateWordListProgressCommand, CreateWordListProgressCommand.Result>
    {
        public override CreateWordListProgressCommand.Result Handle(CreateWordListProgressCommand command)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            var wordListProgress = wordList.CreateProgressForUser(command.OwnerId);
            Session.Store(wordListProgress);
            return new CreateWordListProgressCommand.Result(wordListProgress);
        }

        public override bool CanExecute(CreateWordListProgressCommand command, User currentUser)
        {
            return true;
        }
    }
}