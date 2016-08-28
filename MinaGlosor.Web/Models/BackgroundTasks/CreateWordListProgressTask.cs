namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class CreateWordListProgressTask
    {
        public CreateWordListProgressTask(string wordListId, string ownerId)
        {
            WordListId = wordListId;
            OwnerId = ownerId;
        }

        public string WordListId { get; private set; }

        public string OwnerId { get; private set; }
    }
}