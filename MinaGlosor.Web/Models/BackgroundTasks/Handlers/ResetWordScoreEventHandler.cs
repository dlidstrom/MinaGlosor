using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class ResetWordScoreEventHandler : BackgroundTaskHandler<ResetWordScoreEvent>
    {
        public override void Handle(ResetWordScoreEvent task)
        {
            ExecuteCommand(new ResetWordScoreCommand(task.WordScoreId), task);
        }
    }
}