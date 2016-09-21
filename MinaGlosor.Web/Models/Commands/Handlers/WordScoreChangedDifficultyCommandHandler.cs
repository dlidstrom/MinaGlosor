using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class WordScoreChangedDifficultyCommandHandler : CommandHandlerBase<WordScoreChangedDifficultyCommand, object>
    {
        public override object Handle(WordScoreChangedDifficultyCommand command)
        {
            // load progress model, update it
            var progressId = Progress.GetIdFromWordListForUser(command.WordListId, command.OwnerId);
            var progress = Session.Load<Progress>(progressId);
            progress.UpdateDifficultyCounts(command.WordScoreDifficultyLifecycle);
            return new object();
        }

        public override bool CanExecute(WordScoreChangedDifficultyCommand command, User currentUser)
        {
            return true;
        }
    }
}