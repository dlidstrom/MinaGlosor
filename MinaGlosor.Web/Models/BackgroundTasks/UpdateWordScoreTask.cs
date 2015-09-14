using System;

namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class UpdateWordScoreTask
    {
        public UpdateWordScoreTask(string wordId, string wordListId, ConfidenceLevel confidenceLevel, string ownerId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");

            WordId = wordId;
            WordListId = wordListId;
            ConfidenceLevel = confidenceLevel;
            OwnerId = ownerId;
        }

        public string WordId { get; private set; }

        public string WordListId { get; private set; }

        public ConfidenceLevel ConfidenceLevel { get; private set; }

        public string OwnerId { get; private set; }

        public override string ToString()
        {
            return string.Format("WordId: {0}, WordListId: {1}, ConfidenceLevel: {2}, OwnerId: {3}", WordId, WordListId, ConfidenceLevel, OwnerId);
        }
    }
}