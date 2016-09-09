using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class UpdateWordListNameCommandHandler : CommandHandlerBase<UpdateWordListNameCommand, object>
    {
        public override object Handle(UpdateWordListNameCommand command)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            wordList.UpdateName(command.WordListName);
            return new object();
        }

        public override bool CanExecute(UpdateWordListNameCommand command, User currentUser)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            var canExecute = wordList.OwnerId == currentUser.Id;
            return canExecute;
        }
    }
}