using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class WordScoreRegisteredTaskHandler : BackgroundTaskHandler<WordScoreRegisteredTask>
    {
        public override void Handle(WordScoreRegisteredTask task)
        {
            ExecuteCommand(new WordScoreRegisteredCommand(task.WordId, task.OwnerId), task);
        }
    }
}