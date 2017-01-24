using System;
using MinaGlosor.Web.Models.Domain.ProgressModel.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class AddWordTaskHandler : BackgroundTaskHandler<AddWordTask>
    {
        public override void Handle(AddWordTask task, Guid correlationId)
        {
            ExecuteCommand(new UpdateProgressAfterAddWordCommand(task.ProgressId, task.NumberOfWords), task, correlationId);
        }
    }
}