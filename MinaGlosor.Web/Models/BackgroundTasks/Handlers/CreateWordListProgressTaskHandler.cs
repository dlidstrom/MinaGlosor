using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class CreateWordListProgressTaskHandler : BackgroundTaskHandler<CreateWordListProgressTask>
    {
        public override void Handle(CreateWordListProgressTask task)
        {
            ExecuteCommand(new CreateWordListProgressCommand(task.WordListId, task.OwnerId), task);
        }
    }
}