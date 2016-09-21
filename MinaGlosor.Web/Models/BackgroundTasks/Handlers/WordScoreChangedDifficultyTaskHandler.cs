using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class WordScoreChangedDifficultyTaskHandler : BackgroundTaskHandler<WordScoreChangedDifficultyTask>
    {
        public override void Handle(WordScoreChangedDifficultyTask task)
        {
            var command = new WordScoreChangedDifficultyCommand(
                task.WordScoreId,
                task.WordListId,
                task.OwnerId,
                task.WordDifficulty,
                task.WordScoreDifficultyLifecycle);
            ExecuteCommand(command, task);
        }
    }
}