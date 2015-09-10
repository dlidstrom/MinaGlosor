using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class AddWordToWordListCommandHandler : CommandHandlerBase<AddWordToWordListCommand, object>
    {
        public override object Handle(AddWordToWordListCommand command)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            wordList.AddWord();
            return null;
        }

        public override bool CanExecute(AddWordToWordListCommand command, User currentUser)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            var hasAccess = wordList.HasAccess(currentUser.Id);
            return hasAccess;
        }
    }
}