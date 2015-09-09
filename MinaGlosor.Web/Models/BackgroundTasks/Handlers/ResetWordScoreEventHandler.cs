using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class ResetWordScoreEventHandler : BackgroundTaskHandler<ResetWordScoreTask>
    {
        public override void Handle(ResetWordScoreTask task)
        {
            ExecuteCommand(new ResetWordScoreCommand(task.WordScoreId), task);
        }
    }
}