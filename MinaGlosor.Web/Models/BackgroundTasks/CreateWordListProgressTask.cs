namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class CreateWordListProgressTask
    {
        public CreateWordListProgressTask(string modelId, string ownerId)
        {
            ModelId = modelId;
            OwnerId = ownerId;
        }

        public string ModelId { get; private set; }

        public string OwnerId { get; private set; }
    }
}