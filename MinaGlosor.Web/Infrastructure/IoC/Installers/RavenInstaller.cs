using System;
using System.IO;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models;
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

        private Func<IDocumentStore> CreateDocumentStore { get; set; }

        private bool InitializeIndexes { get; set; }

        public static RavenInstaller CreateForTests()
        {
            return new RavenInstaller
                {
                    CreateDocumentStore = () => new EmbeddableDocumentStore { RunInMemory = true },
                    InitializeIndexes = true
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
                    CreateDocumentStore = () => embeddableDocumentStore
                };
        }

        public static IWindsorInstaller CreateForServer(string connectionStringName)
        {
            if (connectionStringName == null) throw new ArgumentNullException("connectionStringName");
            return new RavenInstaller
                {
                    CreateDocumentStore = () => new DocumentStore { ConnectionStringName = connectionStringName },
                    InitializeIndexes = true
                };
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            TracingLogger.Information("Initializing document store");
            var documentStore = CreateDocumentStore().Initialize();
            TracingLogger.Information("Initializing document store done");
            documentStore.Conventions.DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;
            documentStore.Conventions.MaxNumberOfRequestsPerSession = 1024;
            if (InitializeIndexes || ShouldInitializeIndexes(documentStore, Application.GetAppVersion()))
            {
                TracingLogger.Information("Initializing indexes");
                IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), documentStore);
                TracingLogger.Information("Initializing indexes done");
            }

            container.Register(Component.For<IDocumentStore>().Instance(documentStore));
            container.Register(
                Component.For<IDocumentSession>()
                         .UsingFactoryMethod(k => CreateSession(k.Resolve<IDocumentStore>()))
                         .LifestyleScoped());
        }

        private static IDocumentSession CreateSession(IDocumentStore documentStore)
        {
            var documentSession = documentStore.OpenSession();
            documentSession.Advanced.UseOptimisticConcurrency = true;
            return documentSession;
        }

        private static bool ShouldInitializeIndexes(IDocumentStore documentStore, string version)
        {
            using (var session = documentStore.OpenSession())
            {
                var config = session.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
                if (config == null)
                {
                    TracingLogger.Information(
                        EventIds.Informational_ApplicationLog_3XXX.Web_CreateWebsiteConfig_3004,
                        "Creating website config");
                    config = new WebsiteConfig();
                    session.Store(config);
                }

                TracingLogger.Information(
                    EventIds.Informational_ApplicationLog_3XXX.Web_CheckVersion_3005,
                    "IndexCreatedVersion: {0} NewVersion: {1}",
                    config.IndexCreatedVersion,
                    version);
                var newVersion = config.IndexCreatedVersion != version;
                config.SetIndexCreatedVersion(version);
                session.SaveChanges();

                return newVersion;
            }
        }
    }
}