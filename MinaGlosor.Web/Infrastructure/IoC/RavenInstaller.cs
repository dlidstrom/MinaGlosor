using System;
using System.IO;
using Castle.Core;
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
        private readonly ApplicationMode applicationMode;
        private readonly LifestyleType lifestyleType;

        public RavenInstaller()
        {
            applicationMode = MvcApplication.Mode;
            lifestyleType = LifestyleType.PerWebRequest;
        }

        public RavenInstaller(ApplicationMode mode, LifestyleType lifestyleType)
        {
            applicationMode = mode;
            this.lifestyleType = lifestyleType;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IDocumentStore>()
                         .UsingFactoryMethod(CreateStore)
                         .LifestyleSingleton());
            container.Register(
                Component.For<IDocumentSession>()
                         .UsingFactoryMethod(CreateSession)
                         .LifeStyle.Is(lifestyleType));
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

        private static IDocumentStore CreateInMemoryDocumentStore()
        {
            return new EmbeddableDocumentStore
            {
                RunInMemory = true
            };
        }

        private IDocumentStore CreateStore()
        {
            IDocumentStore documentStore;
            switch (applicationMode)
            {
                case ApplicationMode.Debug:
                    documentStore = new DocumentStore { ConnectionStringName = "RavenDB" };
                    break;

                case ApplicationMode.Release:
                    documentStore = CreateEmbeddableDocumentStore();
                    break;

                case ApplicationMode.Test:
                    documentStore = CreateInMemoryDocumentStore();
                    break;

                default:
                    throw new ApplicationException("Mode not yet implemented");
            }

            InitializeStore(documentStore);
            return documentStore;
        }
    }
}