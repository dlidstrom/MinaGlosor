using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MinaGlosor.Web.Models.BackgroundTasks.Handlers;

namespace MinaGlosor.Web.Infrastructure.IoC.Installers
{
    public class BackgroundTaskHandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                       .BasedOn(typeof(BackgroundTaskHandler<>))
                       .WithServiceBase()
                       .LifestyleTransient());
        }
    }
}