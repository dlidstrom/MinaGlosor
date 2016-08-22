using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class WordListProgressTaskHandler : BackgroundTaskHandler<WordExpiredTask>
    {
        public override void Handle(WordExpiredTask task)
        {
            ExecuteCommand(new WordExpiredCommand(task.WordId, task.OwnerId), task);
        }
    }
}