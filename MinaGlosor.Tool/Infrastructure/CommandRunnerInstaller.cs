using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MinaGlosor.Tool.Commands;

namespace MinaGlosor.Tool.Infrastructure
{
    public class CommandRunnerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                       .BasedOn<ICommandRunner>()
                       .Configure(x => x.LifestyleTransient().Named(x.Implementation.Name.Replace("CommandRunner", string.Empty)))
                       .WithServiceBase());
        }
    }
}