namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class ResetWordScoreEvent
    {
        public ResetWordScoreEvent(string wordScoreId)
        {
            WordScoreId = wordScoreId;
        }

        public string WordScoreId { get; private set; }
    }
}