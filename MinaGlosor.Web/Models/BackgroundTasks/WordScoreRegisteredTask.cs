namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class WordScoreRegisteredTask
    {
        public WordScoreRegisteredTask(string wordId, string ownerId)
        {
            WordId = wordId;
            OwnerId = ownerId;
        }

        public string WordId { get; private set; }

        public string OwnerId { get; private set; }
    }
}