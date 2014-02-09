using System;
using System.IO;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

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
            IDocumentStore documentStore;
            switch (MvcApplication.Mode)
            {
                case ApplicationMode.Debug:
                    documentStore = new DocumentStore { ConnectionStringName = "RavenDB" };
                    break;

                case ApplicationMode.Release:
                    documentStore = CreateEmbeddableDocumentStore();
                    break;

                default:
                    throw new ApplicationException("Mode not yet implemented");
            }

            InitializeStore(documentStore);
            return documentStore;
        }

        private static EmbeddableDocumentStore CreateEmbeddableDocumentStore()
        {
            var path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var dataDirectory = Path.Combine(path, "Database");
            var documentStore = new EmbeddableDocumentStore
                {
                    DataDirectory = dataDirectory
                };
            documentStore.Configuration.MemoryCacheLimitMegabytes = 256;
            return documentStore;
        }

        private static void InitializeStore(IDocumentStore documentStore)
        {
            documentStore.Initialize();
            documentStore.Conventions.IdentityPartsSeparator = "-";
            IndexCreator.CreateIndexes(documentStore);
        }

        private static IDocumentSession CreateSession(IKernel kernel)
        {
            var documentSession = kernel.Resolve<IDocumentStore>().OpenSession();
            documentSession.Advanced.UseOptimisticConcurrency = true;
            return documentSession;
        }
    }
}