using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.ProgressModel;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    // todo rename to better name move to progress command handler
    public class WordExpiredCommandHandler : ICommandHandler<WordExpiredCommand, object>
    {
        public IDocumentSession Session { get; set; }

        public object Handle(WordExpiredCommand command)
        {
            var word = Session.Load<Word>(command.WordId);
            var wordList = Session.Load<WordList>(word.WordListId);
            var progressId = Progress.GetIdFromWordListForUser(word.WordListId, command.OwnerId);
            var progress = Session.Load<Progress>(progressId);
            progress.WordHasExpired(wordList.NumberOfWords);

            return new object();
        }

        public bool CanExecute(WordExpiredCommand command, User currentUser)
        {
            return true;
        }
    }
}