using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MinaGlosor.Web.Models.DomainEvents;

namespace MinaGlosor.Web.Infrastructure.IoC.Installers
{
    public class HandlersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                       .BasedOn(typeof(IHandle<>))
                       .WithServiceAllInterfaces()
                       .LifestyleTransient());
        }
    }
}