namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class ResetWordScoreTask
    {
        public ResetWordScoreTask(string wordScoreId)
        {
            WordScoreId = wordScoreId;
        }

        public string WordScoreId { get; private set; }
    }
}