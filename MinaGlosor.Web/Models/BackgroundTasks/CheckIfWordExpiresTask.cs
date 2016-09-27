namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class CheckIfWordExpiresTask
    {
        public CheckIfWordExpiresTask(string wordScoreId, WordDifficultyUpdate wordDifficultyUpdate)
        {
            WordScoreId = wordScoreId;
            WordDifficultyUpdate = wordDifficultyUpdate;
        }

        public string WordScoreId { get; private set; }
        public WordDifficultyUpdate WordDifficultyUpdate { get; private set; }
    }
}