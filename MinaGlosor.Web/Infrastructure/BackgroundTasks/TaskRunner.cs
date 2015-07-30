using System;
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
using Raven.Client;
using Raven.Client.Linq;
using Timer = System.Timers.Timer;

namespace MinaGlosor.Web.Infrastructure.BackgroundTasks
{
    public class TaskRunner : IRegisteredObject
    {
        private readonly IKernel kernel;
        private readonly IDocumentStore documentStore;
        private readonly Timer timer;
        private readonly object locker = new object();

        public TaskRunner(IKernel kernel, IDocumentStore documentStore, int taskRunnerPollingIntervalMillis)
        {
            this.kernel = kernel;
            this.documentStore = documentStore;

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

        public event EventHandler ProcessedTasks;

        public IDisposable PauseScoped()
        {
            timer.Stop();
            return new EnableDisposable { Timer = timer };
        }

        public void Stop(bool immediate)
        {
            try
            {
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
            if (immediate)
            {
                HostingEnvironment.UnregisterObject(this);
            }
        }

        protected virtual void OnProcessedTasks()
        {
            var handler = ProcessedTasks;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (Monitor.TryEnter(locker) == false)
            {
                TracingLogger.Warning(EventIds.Warning_Transient_4XXX.Web_TaskInProcess_4003, "Abort: Task in process");
                return;
            }

            try
            {
                PerformWork();
                OnProcessedTasks();
            }
            catch (Exception e)
            {
                TracingLogger.Error(EventIds.Error_Permanent_5XXX.Web_UnhandledException_5000, e);
            }
            finally
            {
                timer.Start();
                Monitor.Exit(locker);
            }
        }

        private void PerformWork()
        {
            try
            {
                using (var session = documentStore.OpenSession())
                {
                    if (ProcessTask(session))
                        session.SaveChanges();
                }
            }
            catch (Exception e)
            {
                TracingLogger.Error(EventIds.Error_Permanent_5XXX.Web_UnhandledException_5000, e);
                ErrorLog.GetDefault(null).Log(new Error(e));
            }
        }

        private bool ProcessTask(IDocumentSession session)
        {
            var task = session.Query<BackgroundTask, BackgroundTasksIndex>()
                              .Where(x => x.IsFinished == false && x.IsFailed == false)
                              .OrderBy(x => x.NextTry)
                              .FirstOrDefault();
            if (task == null) return false;

            object handler = null;
            try
            {
                using (new ModelContext(task.CorrelationId))
                using (new ActivityScope(EventIds.Informational_ApplicationLog_3XXX.Web_StartTask_3007, EventIds.Informational_ApplicationLog_3XXX.Web_EndTask_3008, task.ToString()))
                using (kernel.BeginScope())
                {
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

            return true;
        }

        private class EnableDisposable : IDisposable
        {
            public Timer Timer { private get; set; }

            public void Dispose()
            {
                Timer.Start();
            }
        }
    }
}