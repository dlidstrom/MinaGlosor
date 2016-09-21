using System;

namespace MinaGlosor.Web.Models.BackgroundTasks
{
    public class WordScoreChangedDifficultyTask
    {
        public WordScoreChangedDifficultyTask(
            string wordScoreId,
            string wordListId,
            string ownerId,
            WordDifficulty wordDifficulty,
            WordScoreDifficultyLifecycle wordScoreDifficultyLifecycle)
        {
            if (wordScoreId == null) throw new ArgumentNullException("wordScoreId");
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            WordScoreId = wordScoreId;
            WordListId = wordListId;
            OwnerId = ownerId;
            WordDifficulty = wordDifficulty;
            WordScoreDifficultyLifecycle = wordScoreDifficultyLifecycle;
        }

        public string WordScoreId { get; private set; }

        public string WordListId { get; private set; }

        public string OwnerId { get; private set; }

        public WordDifficulty WordDifficulty { get; private set; }

        public WordScoreDifficultyLifecycle WordScoreDifficultyLifecycle { get; private set; }
    }
}