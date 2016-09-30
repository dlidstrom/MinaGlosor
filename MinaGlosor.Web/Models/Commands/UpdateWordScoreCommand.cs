using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class UpdateWordScoreCommand : ICommand<object>
    {
        public UpdateWordScoreCommand(string wordScoreId, string ownerId, string wordId, WordScoreDifficultyLifecycle wordScoreDifficultyLifecycle)
        {
            WordScoreId = wordScoreId;
            OwnerId = ownerId;
            WordId = wordId;
            WordScoreDifficultyLifecycle = wordScoreDifficultyLifecycle;
        }

        public string WordScoreId { get; private set; }

        public string OwnerId { get; private set; }

        public string WordId { get; private set; }

        public WordScoreDifficultyLifecycle WordScoreDifficultyLifecycle { get; private set; }
    }
}