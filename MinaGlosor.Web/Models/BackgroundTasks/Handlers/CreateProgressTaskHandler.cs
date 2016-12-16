using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class CreateProgressTaskHandler : BackgroundTaskHandler<CreateProgressTask>
    {
        public override void Handle(CreateProgressTask task)
        {
            ExecuteCommand(new CreateProgressCommand(task.WordListId, task.OwnerId), task);
        }
    }
}