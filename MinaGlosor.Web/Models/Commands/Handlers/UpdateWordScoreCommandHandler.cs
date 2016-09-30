using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class UpdateWordScoreCommandHandler : CommandHandlerBase<UpdateWordScoreCommand, object>
    {
        public override object Handle(UpdateWordScoreCommand command)
        {
            var word = Session.Load<Word>(command.WordId);
            var id = Progress.GetIdFromWordListForUser(word.WordListId, command.OwnerId);
            var progress = Session.Load<Progress>(id);
            progress.UpdateDifficultyCounts(command.WordScoreDifficultyLifecycle);
            return new object();
        }

        public override bool CanExecute(UpdateWordScoreCommand command, User currentUser)
        {
            return true;
        }
    }
}