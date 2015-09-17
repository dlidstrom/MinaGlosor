using Castle.Facilities.Startable;
using Castle.Windsor;
using MinaGlosor.Web.Infrastructure.IoC.Installers;

namespace MinaGlosor.Web.Infrastructure.IoC
{
    public static class ContainerFactory
    {
        public static IWindsorContainer Create(int taskRunnerPollingIntervalMillis)
        {
            var container = new WindsorContainer();
            container.AddFacility<StartableFacility>(x => x.DeferredStart());
            container.Install(
                new AdminCommandHandlerInstaller(),
                new ControllerInstaller(),
                new WindsorWebApiInstaller(),
                new HandlersInstaller(),
                new BackgroundTaskHandlerInstaller(),
                new TaskRunnerInstaller(taskRunnerPollingIntervalMillis),
                new CommandQueryInstaller(),
                new ControllerFactoryInstaller(),
#if DEBUG
                RavenInstaller.CreateForServer("RavenDB")
#else
                RavenInstaller.CreateForEmbedded()
#endif
                );
            return container;
           
        }
    }
}