using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web.Hosting;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using Elmah;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.BackgroundTasks;
using MinaGlosor.Web.Models.BackgroundTasks.Handlers;
using Raven.Abstractions;
using Raven.Client;
using Timer = System.Timers.Timer;

namespace MinaGlosor.Web.Infrastructure.BackgroundTasks
{
    [DebuggerDisplay("{uniqueId}")]
    public class TaskRunner : IRegisteredObject
    {
        private readonly Guid uniqueId = Guid.NewGuid();
        private readonly IKernel kernel;
        private readonly Timer timer;
        private readonly object locker = new object();
        private volatile bool exiting;

        public TaskRunner(IKernel kernel, int taskRunnerPollingIntervalMillis)
        {
            this.kernel = kernel;

            timer = new Timer
            {
                Interval = taskRunnerPollingIntervalMillis,
                AutoReset = false
            };
            timer.Elapsed += TimerOnElapsed;
            timer.Start();

            TracingLogger.Information(EventIds.Informational_Preliminary_1XXX.Web_Register_TaskRunner_1002, "Registering with HostingEnvironment");
            HostingEnvironment.RegisterObject(this);
        }

        public event EventHandler<QueueStatusEventArgs> ProcessedTasks;

        public void Stop(bool immediate)
        {
            try
            {
                exiting = true;
                timer.Stop();

                if (Monitor.TryEnter(locker, 10000))
                {
                    // TODO Correct?
                    Monitor.Exit(locker);
                }
                else
                {
                    TracingLogger.Warning(EventIds.Warning_Transient_4XXX.Web_TaskRunner_TimeOut_4004, "Failed to stop on time");
                }
            }
            catch (Exception e)
            {
                TracingLogger.Error(EventIds.Error_Permanent_5XXX.Web_UnhandledException_5000, e);
            }

            TracingLogger.Information(EventIds.Information_Finalization_8XXX.Web_Unregister_TaskRunner_8002, "Unregistering TaskRunner");
            HostingEnvironment.UnregisterObject(this);
        }

        protected virtual void OnProcessedTasks(QueueStatusEventArgs eventArgs)
        {
            var handler = ProcessedTasks;
            if (handler != null)
            {
                TracingLogger.Information("Signalling tasks done");
                handler(this, eventArgs);
            }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (Monitor.TryEnter(locker) == false)
            {
                TracingLogger.Warning(EventIds.Warning_Transient_4XXX.Web_TaskInProcess_4003, "Abort: Task in process");
                if (exiting == false) timer.Start();
                return;
            }

            try
            {
                var queueStatus = PerformWork();
                while (queueStatus.TasksToBeProcessedNow > 0)
                {
                    queueStatus = PerformWork();
                }

                OnProcessedTasks(new QueueStatusEventArgs(queueStatus));
            }
            catch (Exception e)
            {
                TracingLogger.Error(EventIds.Error_Permanent_5XXX.Web_UnhandledException_5000, e);
            }
            finally
            {
                if (exiting == false) timer.Start();
                Monitor.Exit(locker);
            }
        }

        private QueueStatus PerformWork()
        {
            try
            {
                using (kernel.BeginScope())
                using (var session = kernel.Resolve<IDocumentSession>())
                {
                    var queueStatus = ProcessTask(session);
                    if (queueStatus.TasksToBeProcessedNow > 0)
                    {
                        session.SaveChanges();
                        return new QueueStatus(
                            queueStatus.TotalTasksInQueueToBeProcessed,
                            queueStatus.TasksToBeProcessedNow - 1);
                    }

                    return new QueueStatus(queueStatus.TotalTasksInQueueToBeProcessed, queueStatus.TasksToBeProcessedNow);
                }
            }
            catch (Exception e)
            {
                TracingLogger.Error(EventIds.Error_Permanent_5XXX.Web_UnhandledException_5000, e);
                ErrorLog.GetDefault(null).Log(new Error(e));
                return new QueueStatus(0, 0);
            }
        }

        private QueueStatus ProcessTask(IDocumentSession session)
        {
            var utcNow = SystemTime.UtcNow;
            var totalTasksInQueueToBeProcessed = session.Query<BackgroundTask, BackgroundTasksIndex>()
                                                        .Customize(x => x.WaitForNonStaleResults())
                                                        .Where(x => x.IsFinished == false && x.IsFailed == false)
                                                        .ToArray();
            var taskToBeProcessedNow = session.Query<BackgroundTask, BackgroundTasksIndex>()
                                              .Customize(x => x.WaitForNonStaleResults())
                                              .Where(x => x.IsFinished == false
                                                  && x.IsFailed == false
                                                  && x.NextTry <= utcNow)
                                              .OrderBy(x => x.NextTry)
                                              .ToArray();
            TracingLogger.Information(
                "{0} - Tasks to be processed: {1}, total queued: {2}",
                utcNow,
                taskToBeProcessedNow.Length,
                totalTasksInQueueToBeProcessed.Length);
            if (taskToBeProcessedNow.Any() == false)
            {
                return new QueueStatus(totalTasksInQueueToBeProcessed.Length, 0);
            }

            var task = taskToBeProcessedNow.First();
            object handler = null;
            try
            {
                using (new ModelContext(task.CorrelationId))
                using (new ActivityScope(EventIds.Informational_ApplicationLog_3XXX.Web_StartTask_3007, EventIds.Informational_ApplicationLog_3XXX.Web_EndTask_3008, task.ToString()))
                {
                    TracingLogger.Information("Handling task " + task.GetInfo());
                    var handlerType = typeof(BackgroundTaskHandler<>).MakeGenericType(task.Body.GetType());
                    handler = kernel.Resolve(handlerType);
                    var method = handler.GetType().GetMethod("Handle");
                    method.Invoke(handler, new[] { task.Body });
                    task.MarkFinished();
                }
            }
            catch (Exception e)
            {
                TracingLogger.Error(EventIds.Error_Permanent_5XXX.Web_UnhandledException_5000, e);
                ErrorLog.GetDefault(null).Log(new Error(e));
                task.UpdateNextTry(e);
                TracingLogger.Information("Task failed attempt #{0}", task.Retries);
            }
            finally
            {
                if (handler != null) kernel.ReleaseComponent(handler);
            }

            return new QueueStatus(totalTasksInQueueToBeProcessed.Length, taskToBeProcessedNow.Length);
        }

        public class QueueStatus
        {
            public QueueStatus(int totalTasksInQueueToBeProcessed, int tasksToBeProcessedNow)
            {
                TotalTasksInQueueToBeProcessed = totalTasksInQueueToBeProcessed;
                TasksToBeProcessedNow = tasksToBeProcessedNow;
            }

            public int TotalTasksInQueueToBeProcessed { get; private set; }

            public int TasksToBeProcessedNow { get; private set; }
        }

        public class QueueStatusEventArgs : EventArgs
        {
            public QueueStatusEventArgs(QueueStatus queueStatus)
            {
                QueueStatus = queueStatus;
            }

            public QueueStatus QueueStatus { get; private set; }
        }
    }
}