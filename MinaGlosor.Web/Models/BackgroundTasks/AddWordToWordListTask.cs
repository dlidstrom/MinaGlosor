namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class AddWordToWordListTask
    {
        public AddWordToWordListTask(string wordListId)
        {
            WordListId = wordListId;
        }

        public string WordListId { get; private set; }

        public override string ToString()
        {
            return string.Format("WordListId: {0}", WordListId);
        }
    }
}