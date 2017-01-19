using MinaGlosor.Web.Models.Domain.ProgressModel.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class ProgressSortOrderTaskHandler : BackgroundTaskHandler<ProgressNumberOfWordsSortOrderTask>
    {
        public override void Handle(ProgressNumberOfWordsSortOrderTask task)
        {
            ExecuteCommand(new ProgressNumberOfWordsSortOrderCommand(), task);
        }
    }
}