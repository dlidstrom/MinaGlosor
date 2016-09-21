using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class WordScoreChangedDifficultyCommand : ICommand<object>
    {
        public WordScoreChangedDifficultyCommand(
            string wordScoreId,
            string wordListId,
            string ownerId,
            WordDifficulty wordDifficulty,
            WordScoreDifficultyLifecycle wordScoreDifficultyLifecycle)
        {
            if (wordScoreId == null) throw new ArgumentNullException("wordScoreId");
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