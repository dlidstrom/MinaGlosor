using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class UpdateWordScoreTaskHandler : BackgroundTaskHandler<UpdateWordScoreTask>
    {
        public override void Handle(UpdateWordScoreTask task)
        {
            var command = new UpdateWordScoreCommand(task.OwnerId, task.WordId, task.WordListId, task.ConfidenceLevel);
            ExecuteCommand(command, task);
        }
    }
}