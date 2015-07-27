namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class ResetWordScoreEvent
    {
        public string WordScoreId { get; private set; }

        public ResetWordScoreEvent(string wordScoreId)
        {
            WordScoreId = wordScoreId;
        }
    }
}