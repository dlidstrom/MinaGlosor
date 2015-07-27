using System.Linq;
using MinaGlosor.Web.Models.BackgroundTasks;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Infrastructure.BackgroundTasks
{
    public class BackgroundTasksIndex : AbstractIndexCreationTask<BackgroundTask>
    {
        public BackgroundTasksIndex()
        {
            Map = tasks => from task in tasks
                           select new
                           {
                               task.NextTry,
                               task.IsFinished,
                               task.IsFailed
                           };
        }
    }
}