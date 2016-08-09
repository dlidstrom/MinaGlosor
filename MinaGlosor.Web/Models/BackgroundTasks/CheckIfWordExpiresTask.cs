namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class CheckIfWordExpiresTask
    {
        public CheckIfWordExpiresTask(string wordScoreId)
        {
            WordScoreId = wordScoreId;
        }

        public string WordScoreId { get; private set; }
    }
}