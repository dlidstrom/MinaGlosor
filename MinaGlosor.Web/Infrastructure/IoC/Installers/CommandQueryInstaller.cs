using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace MinaGlosor.Web.Infrastructure.IoC.Installers
{
    public class CommandQueryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<QueryExecutor>().LifestyleTransient());
            container.Register(Component.For<CommandExecutor>().LifestyleTransient());
            container.Register(
                Classes.FromThisAssembly()
                       .BasedOn(typeof(CommandHandlerBase<,>))
                       .WithServiceBase()
                       .LifestyleTransient());
            container.Register(
                Classes.FromThisAssembly()
                       .BasedOn(typeof(QueryHandlerBase<,>))
                       .WithServiceBase()
                       .LifestyleTransient());
        }
    }
}