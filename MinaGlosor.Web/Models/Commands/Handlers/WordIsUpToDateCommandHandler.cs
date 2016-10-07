using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.ProgressModel;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class WordIsUpToDateCommandHandler : CommandHandlerBase<WordIsUpToDateCommand, object>
    {
        public override object Handle(WordIsUpToDateCommand command)
        {
            var word = Session.Load<Word>(command.WordId);
            var wordList = Session.Load<WordList>(word.WordListId);
            var id = Progress.GetIdFromWordListForUser(wordList.Id, command.OwnerId);
            var progress = Session.Load<Progress>(id);
            progress.WordIsUpToDate(wordList.NumberOfWords);
            return new object();
        }

        public override bool CanExecute(WordIsUpToDateCommand command, User currentUser)
        {
            return true;
        }
    }
}