using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class CheckIfWordExpiresTaskHandler : BackgroundTaskHandler<CheckIfWordExpiresTask>
    {
        public override void Handle(CheckIfWordExpiresTask task)
        {
            ExecuteCommand(new CheckIfWordExpiresCommand(task.WordScoreId), task);
        }
    }
}