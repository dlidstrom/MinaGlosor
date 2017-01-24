using System;
using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class ProgressTaskHandler : BackgroundTaskHandler<WordExpiredTask>
    {
        public override void Handle(WordExpiredTask task, Guid correlationId)
        {
            ExecuteCommand(new WordExpiredCommand(task.WordId, task.OwnerId), task, correlationId);
        }
    }
}