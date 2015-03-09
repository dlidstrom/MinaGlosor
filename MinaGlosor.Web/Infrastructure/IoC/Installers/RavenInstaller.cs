using System;
using System.IO;
using System.Reflection;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Infrastructure.IoC.Installers
{
    public class RavenInstaller : IWindsorInstaller
    {
        private RavenInstaller()
        {
        }

        protected LifestyleType Lifestyle { get; set; }

        private Func<IDocumentStore> CreateDocumentStore { get; set; }

        public static RavenInstaller CreateForTests()
        {
            return new RavenInstaller
                {
                    CreateDocumentStore = () => new EmbeddableDocumentStore { RunInMemory = true },
                    Lifestyle = LifestyleType.Scoped
                };
        }

        public static IWindsorInstaller CreateForEmbedded()
        {
            var appDataPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var dataDirectory = Path.Combine(appDataPath, "Database");
            var embeddableDocumentStore = new EmbeddableDocumentStore
                {
                    DataDirectory = dataDirectory
                };
            embeddableDocumentStore.Configuration.MemoryCacheLimitMegabytes = 256;
            return new RavenInstaller
                {
                    CreateDocumentStore = () => embeddableDocumentStore,
                    Lifestyle = LifestyleType.PerWebRequest
                };
        }

        public static IWindsorInstaller CreateForServer(string connectionStringName)
        {
            if (connectionStringName == null) throw new ArgumentNullException("connectionStringName");
            return new RavenInstaller
                {
                    CreateDocumentStore = () => new DocumentStore { ConnectionStringName = connectionStringName },
                    Lifestyle = LifestyleType.PerWebRequest
                };
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var documentStore = CreateDocumentStore().Initialize();
            documentStore.Conventions.DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;
            documentStore.Conventions.MaxNumberOfRequestsPerSession = 1024;
            IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), documentStore);
            container.Register(Component.For<IDocumentStore>().Instance(documentStore));
            container.Register(
                Component.For<IDocumentSession>()
                         .UsingFactoryMethod(k => CreateSession(k.Resolve<IDocumentStore>()))
                         .LifeStyle.Is(Lifestyle));
        }

        private static IDocumentSession CreateSession(IDocumentStore documentStore)
        {
            var documentSession = documentStore.OpenSession();
            documentSession.Advanced.UseOptimisticConcurrency = true;
            return documentSession;
        }
    }
}