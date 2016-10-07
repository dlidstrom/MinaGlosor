using System;

namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class AddWordTask
    {
        public AddWordTask(string progressId, int numberOfWords)
        {
            if (progressId == null) throw new ArgumentNullException("progressId");
            ProgressId = progressId;
            NumberOfWords = numberOfWords;
        }

        public string ProgressId { get; private set; }

        public int NumberOfWords { get; private set; }
    }
}