using System;
using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class AddWordToWordListTaskHandler : BackgroundTaskHandler<AddWordToWordListTask>
    {
        public override void Handle(AddWordToWordListTask task, Guid correlationId)
        {
            ExecuteCommand(new AddWordToWordListCommand(task.WordListId), task, correlationId);
        }
    }
}