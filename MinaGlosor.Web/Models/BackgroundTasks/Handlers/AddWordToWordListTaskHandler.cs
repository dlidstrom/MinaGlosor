using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class AddWordToWordListTaskHandler : BackgroundTaskHandler<AddWordToWordListTask>
    {
        public override void Handle(AddWordToWordListTask task)
        {
            ExecuteCommand(new AddWordToWordListCommand(task.WordListId), task);
        }
    }
}