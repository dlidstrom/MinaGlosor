using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    // todo rename to better name
    public class WordExpiredCommandHandler : CommandHandlerBase<WordExpiredCommand, object>
    {
        public override object Handle(WordExpiredCommand command)
        {
            var word = Session.Load<Word>(command.WordId);
            var wordList = Session.Load<WordList>(word.WordListId);
            var wordListProgressId = WordListProgress.Model.GetIdFromWordListForUser(word.WordListId, command.OwnerId);
            var wordListProgress = Session.Load<WordListProgress.Model>(wordListProgressId);
            wordListProgress.WordHasExpired(wordList.NumberOfWords);
            return new object();
        }

        public override bool CanExecute(WordExpiredCommand command, User currentUser)
        {
            return true;
        }
    }
}