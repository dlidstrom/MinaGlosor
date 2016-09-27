namespace MinaGlosor.Web.Models
{
    public class WordDifficultyUpdate
    {
        public WordDifficultyUpdate(WordDifficulty nextWordDifficulty, WordScoreDifficultyLifecycle wordScoreDifficultyLifecycle)
        {
            NextWordDifficulty = nextWordDifficulty;
            WordScoreDifficultyLifecycle = wordScoreDifficultyLifecycle;
        }

        public WordDifficulty NextWordDifficulty { get; private set; }

        public WordScoreDifficultyLifecycle WordScoreDifficultyLifecycle { get; private set; }
    }
}