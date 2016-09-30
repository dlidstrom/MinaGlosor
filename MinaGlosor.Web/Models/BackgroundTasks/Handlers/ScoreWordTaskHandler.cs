using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class ScoreWordTaskHandler : BackgroundTaskHandler<ScoreWordTask>
    {
        public override void Handle(ScoreWordTask task)
        {
            var command = new ScoreWordCommand(task.OwnerId, task.WordId, task.WordListId, task.ConfidenceLevel);
            ExecuteCommand(command, task);
        }
    }
}