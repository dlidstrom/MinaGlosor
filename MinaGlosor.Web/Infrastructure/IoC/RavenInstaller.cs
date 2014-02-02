using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Infrastructure.IoC
{
    public static class IndexCreator
    {
        public static void CreateIndexes(IDocumentStore store)
        {
            var indexes = from type in Assembly.GetExecutingAssembly().GetTypes()
                          where
                              type.IsSubclassOf(typeof(AbstractIndexCreationTask))
                          select type;

            var typeCatalog = new TypeCatalog(indexes.ToArray());
            IndexCreation.CreateIndexes(new CompositionContainer(typeCatalog), store);
        }
    }

    public class RavenInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IDocumentStore>().UsingFactoryMethod(CreateStore).LifestyleSingleton());
            container.Register(Component.For<IDocumentSession>().UsingFactoryMethod(CreateSession).LifestylePerWebRequest());
        }

        private static IDocumentStore CreateStore()
        {
            var path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var dataDirectory = Path.Combine(path, "Database");
            var documentStore = new EmbeddableDocumentStore
                {
                    DataDirectory = dataDirectory
                }.Initialize();
            //documentStore.Configuration.MemoryCacheLimitMegabytes = 256;
            IndexCreator.CreateIndexes(documentStore);
            return documentStore;
        }

        private static IDocumentSession CreateSession(IKernel kernel)
        {
            return kernel.Resolve<IDocumentStore>().OpenSession();
        }
    }
}