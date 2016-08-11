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

    public class WordIsUpToDateTaskHandler : BackgroundTaskHandler<WordIsUpToDateTask>
    {
        public override void Handle(WordIsUpToDateTask task)
        {
            ExecuteCommand(new WordIsUpToDateCommand(task.WordId, task.OwnerId), task);
        }
    }
}