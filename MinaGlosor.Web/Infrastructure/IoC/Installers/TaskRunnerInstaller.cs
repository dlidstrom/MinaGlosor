using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MinaGlosor.Web.Infrastructure.BackgroundTasks;

namespace MinaGlosor.Web.Infrastructure.IoC.Installers
{
    public class TaskRunnerInstaller : IWindsorInstaller
    {
        private readonly int taskRunnerPollingIntervalMillis;

        public TaskRunnerInstaller(int taskRunnerPollingIntervalMillis)
        {
            this.taskRunnerPollingIntervalMillis = taskRunnerPollingIntervalMillis;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<TaskRunner>()
                         .LifestyleSingleton()
                         .DependsOn(Dependency.OnValue("taskRunnerPollingIntervalMillis", taskRunnerPollingIntervalMillis))
                         .Start());
        }
    }
}