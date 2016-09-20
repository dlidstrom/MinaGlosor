using System;

namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class UpdateWordScoreDifficultyTask
    {
        public UpdateWordScoreDifficultyTask(WordDifficulty wordDifficulty, string wordId, string ownerId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            WordDifficulty = wordDifficulty;
            WordId = wordId;
            OwnerId = ownerId;
        }

        public WordDifficulty WordDifficulty { get; private set; }

        public string WordId { get; private set; }

        public string OwnerId { get; private set; }
    }
}