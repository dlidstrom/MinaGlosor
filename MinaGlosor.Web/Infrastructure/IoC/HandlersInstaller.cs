using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MinaGlosor.Web.Data.Handlers;

namespace MinaGlosor.Web.Infrastructure.IoC
{
    public class HandlersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                       .BasedOn(typeof(IHandle<>))
                       .WithServiceFromInterface(typeof(IHandle<>))
                       .Configure(c => c.LifestyleTransient()));
        }
    }
}