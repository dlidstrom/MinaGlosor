using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class UpdateWordScoreDifficultyTaskHandler : BackgroundTaskHandler<UpdateWordScoreDifficultyTask>
    {
        public override void Handle(UpdateWordScoreDifficultyTask task)
        {
            ExecuteCommand(new UpdateWordScoreDifficultyCommand(task.ConfidenceLevels, task.WordId, task.OwnerId), task);
        }
    }
}