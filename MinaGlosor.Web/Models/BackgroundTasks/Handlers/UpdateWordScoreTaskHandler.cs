using System;
using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.BackgroundTasks.Handlers
{
    public class UpdateWordScoreTaskHandler : BackgroundTaskHandler<UpdateWordScoreTask>
    {
        public override void Handle(UpdateWordScoreTask task, Guid correlationId)
        {
            ExecuteCommand(new UpdateWordScoreCommand(task.WordScoreId, task.OwnerId, task.WordId, task.WordScoreDifficultyLifecycle), task, correlationId);
        }
    }
}