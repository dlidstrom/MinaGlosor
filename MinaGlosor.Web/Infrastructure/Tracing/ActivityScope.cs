using System;
using System.Diagnostics;

namespace MinaGlosor.Web.Infrastructure.Tracing
{
    public class ActivityScope : IDisposable
    {
        private readonly int stopId;
        private readonly string activityName;
        private readonly Guid oldActivityId;

        public ActivityScope(int startId, int stopId, string activityName)
        {
            this.stopId = stopId;
            this.activityName = activityName;
            oldActivityId = Trace.CorrelationManager.ActivityId;
            Trace.CorrelationManager.ActivityId = SystemGuid.NewSequential;
            TracingLogger.Start(startId, activityName);
        }

        public ActivityScope(int startId, int stopId, string activityName, Guid activityId)
        {
            this.stopId = stopId;
            this.activityName = activityName;
            oldActivityId = Trace.CorrelationManager.ActivityId;
            Trace.CorrelationManager.ActivityId = activityId;
            TracingLogger.Start(startId, activityName);
        }

        public void Dispose()
        {
            TracingLogger.Stop(stopId, activityName);
            Trace.CorrelationManager.ActivityId = oldActivityId;
        }
    }
}