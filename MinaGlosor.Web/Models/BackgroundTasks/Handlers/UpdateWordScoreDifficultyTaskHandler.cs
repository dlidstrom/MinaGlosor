using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class UpdateWordScoreDifficultyTaskHandler : BackgroundTaskHandler<UpdateWordScoreDifficultyTask>
    {
        public override void Handle(UpdateWordScoreDifficultyTask task)
        {
            ExecuteCommand(new UpdateWordScoreDifficultyCommand(task.WordDifficulty, task.WordId, task.OwnerId), task);
        }
    }
}