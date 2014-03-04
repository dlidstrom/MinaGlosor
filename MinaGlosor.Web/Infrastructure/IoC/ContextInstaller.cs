using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MinaGlosor.Web.Data;

namespace MinaGlosor.Web.Infrastructure.IoC
{
    public class ContextInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IDbContext>()
                         .ImplementedBy<Context>()
                         .LifestylePerWebRequest());
        }
    }
}