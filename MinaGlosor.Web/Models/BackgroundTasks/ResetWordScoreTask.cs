namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class ResetWordScoreTask
    {
        public ResetWordScoreTask(string wordScoreId)
        {
            WordScoreId = wordScoreId;
        }

        public string WordScoreId { get; private set; }

        public override string ToString()
        {
            return string.Format("WordScoreId: {0}", WordScoreId);
        }
    }
}