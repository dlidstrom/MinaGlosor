using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MinaGlosor.Web.Models.AdminCommands;

namespace MinaGlosor.Web.Infrastructure.IoC.Installers
{
    public class AdminCommandHandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                       .BasedOn(typeof(IAdminCommandHandler<>))
                       .WithServiceAllInterfaces()
                       .LifestyleTransient());
        }
    }
}