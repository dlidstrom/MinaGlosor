namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class UpdateWordScoreTask
    {
        public UpdateWordScoreTask(string wordScoreId, string wordId, string ownerId, WordScoreDifficultyLifecycle wordScoreDifficultyLifecycle)
        {
            WordScoreId = wordScoreId;
            WordId = wordId;
            OwnerId = ownerId;
            WordScoreDifficultyLifecycle = wordScoreDifficultyLifecycle;
        }

        public string WordScoreId { get; private set; }

        public string WordId { get; private set; }

        public string OwnerId { get; private set; }

        public WordScoreDifficultyLifecycle WordScoreDifficultyLifecycle { get; private set; }
    }
}