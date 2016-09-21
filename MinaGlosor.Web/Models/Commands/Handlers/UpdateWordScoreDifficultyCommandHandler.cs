using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class UpdateWordScoreDifficultyCommandHandler : CommandHandlerBase<UpdateWordScoreDifficultyCommand, object>
    {
        public override object Handle(UpdateWordScoreDifficultyCommand command)
        {
            var wordScore = Session.Query<WordScore, WordScoreIndex>()
                                   .Single(x => x.OwnerId == command.OwnerId && x.WordId == command.WordId);
            wordScore.UpdateDifficultyAfterPractice(command.ConfidenceLevels);
            return new object();
        }

        public override bool CanExecute(UpdateWordScoreDifficultyCommand command, User currentUser)
        {
            return true;
        }
    }
}