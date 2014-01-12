using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;

namespace MinaGlosor.Web.Infrastructure.IoC
{
    public class RavenInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IDocumentStore>().UsingFactoryMethod(CreateStore).LifestyleSingleton());
            container.Register(Component.For<IDocumentSession>().UsingFactoryMethod(CreateSession).LifestylePerWebRequest());
        }

        private static IDocumentStore CreateStore()
        {
            return new DocumentStore
                {
                    ConnectionStringName = "RavenDB"
                }.Initialize();
        }

        private static IDocumentSession CreateSession(IKernel kernel)
        {
            return kernel.Resolve<IDocumentStore>().OpenSession();
        }
    }
}