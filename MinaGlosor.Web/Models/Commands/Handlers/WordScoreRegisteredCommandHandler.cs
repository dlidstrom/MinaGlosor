using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    // todo merge with WordIsUpToDateCommandHandler
    public class WordScoreRegisteredCommandHandler : CommandHandlerBase<WordScoreRegisteredCommand, object>
    {
        public override object Handle(WordScoreRegisteredCommand command)
        {
            var word = Session.Load<Word>(command.WordId);
            var wordList = Session.Load<WordList>(word.WordListId);
            var id = Progress.GetIdFromWordListForUser(wordList.Id, command.OwnerId);
            var progress = Session.Load<Progress>(id);
            progress.NewWordHasBeenPracticed(wordList.NumberOfWords);
            return new object();
        }

        public override bool CanExecute(WordScoreRegisteredCommand command, User currentUser)
        {
            return true;
        }
    }
}