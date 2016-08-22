using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class WordIsUpToDateTaskHandler : BackgroundTaskHandler<WordIsUpToDateTask>
    {
        public override void Handle(WordIsUpToDateTask task)
        {
            ExecuteCommand(new WordIsUpToDateCommand(task.WordId, task.OwnerId), task);
        }
    }
}