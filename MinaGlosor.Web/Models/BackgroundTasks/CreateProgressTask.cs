namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class CreateProgressTask
    {
        public CreateProgressTask(string wordListId, string ownerId)
        {
            WordListId = wordListId;
            OwnerId = ownerId;
        }

        public string WordListId { get; private set; }

        public string OwnerId { get; private set; }
    }
}