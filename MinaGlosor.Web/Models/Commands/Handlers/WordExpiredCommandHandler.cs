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
            var progressId = Progress.GetIdFromWordListForUser(word.WordListId, command.OwnerId);
            var progress = Session.Load<Progress>(progressId);
            progress.WordHasExpired(wordList.NumberOfWords);

            return new object();
        }

        public override bool CanExecute(WordExpiredCommand command, User currentUser)
        {
            return true;
        }
    }
}