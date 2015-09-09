namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class AddWordToWordListTask
    {
        public AddWordToWordListTask(string wordListId)
        {
            WordListId = wordListId;
        }

        public string WordListId { get; set; }
    }
}