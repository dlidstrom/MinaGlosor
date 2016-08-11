using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class WordIsUpToDateCommandHandler : CommandHandlerBase<WordIsUpToDateCommand, object>
    {
        public override object Handle(WordIsUpToDateCommand command)
        {
            var word = Session.Load<Word>(command.WordId);
            var wordList = Session.Load<WordList>(word.WordListId);
            var id = WordListProgress.Model.GetIdFromWordListForUser(wordList.Id, command.OwnerId);
            var wordListProgress = Session.Load<WordListProgress.Model>(id);
            wordListProgress.WordIsUpToDate(wordList.NumberOfWords);
            return new object();
        }

        public override bool CanExecute(WordIsUpToDateCommand command, User currentUser)
        {
            return true;
        }
    }
}