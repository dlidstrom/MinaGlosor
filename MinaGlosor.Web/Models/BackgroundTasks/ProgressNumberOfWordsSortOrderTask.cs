using System;

namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class ProgressNumberOfWordsSortOrderTask
    {
        public ProgressNumberOfWordsSortOrderTask(string wordListId, string ownerId, int numberOfWords)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            WordListId = wordListId;
            OwnerId = ownerId;
            NumberOfWords = numberOfWords;
        }

        public string WordListId { get; private set; }
        public string OwnerId { get; private set; }
        public int NumberOfWords { get; private set; }
    }
}