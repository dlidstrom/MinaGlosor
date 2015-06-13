using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MinaGlosor.Tool.Commands;

namespace MinaGlosor.Tool
{
    public class CommandsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                       .BasedOn<ICommand>()
                       .Configure(x => x.LifestyleTransient().Named(x.Implementation.Name.Replace("Command", string.Empty)))
                       .WithServiceBase());
        }
    }
}