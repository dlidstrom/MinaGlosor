using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class UpdateWordCommandHandler : CommandHandlerBase<UpdateWordCommand, object>
    {
        public override object Handle(UpdateWordCommand command)
        {
            var word = Session.Load<Word>(command.WordId);
            word.Update(command.Text, command.Definition);
            return null;
        }

        public override bool CanExecute(UpdateWordCommand command, User currentUser)
        {
            var word = Session.Load<Word>(command.WordId);
            var wordList = Session.Load<WordList>(word.WordListId);
            var hasAccess = wordList.HasAccess(currentUser.Id);
            return hasAccess;
        }
    }
}