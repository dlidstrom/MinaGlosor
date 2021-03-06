using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public abstract class KeyGeneratorBase
    {
        protected static readonly PluralizationService PluralizationService = PluralizationService.CreateService(new CultureInfo("en"));

        public static string Generate<TModel>(IDocumentSession session) where TModel : DomainModel
        {
            var tag = TransformTypeTagNameToDocumentKeyPrefix(typeof(TModel).Name);
            var keyGenerator = new HiLoKeyGenerator(tag, 4);
            var documentSession = (DocumentSession)session;
            var documentStore = documentSession.DocumentStore;
            var id = keyGenerator.GenerateDocumentKey(documentStore.DatabaseCommands, documentStore.Conventions, null);
            return id;
        }

        protected static string TransformTypeTagNameToDocumentKeyPrefix(string typeTagName)
        {
            var count = typeTagName.Count(char.IsUpper);
            var prefix = count <= 1 ? typeTagName.ToLowerInvariant() : typeTagName;
            var tag = PluralizationService.Pluralize(prefix);
            return tag;
        }
    }

    public class KeyGenerator<TModel> : KeyGeneratorBase where TModel : DomainModel
    {
        private readonly HiLoKeyGenerator keyGenerator;
        private readonly IDocumentStore documentStore;

        public KeyGenerator(IDocumentSession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            var tag = TransformTypeTagNameToDocumentKeyPrefix(typeof(TModel).Name);
            keyGenerator = new HiLoKeyGenerator(tag, 4);
            var documentSession = (DocumentSession)session;
            documentStore = documentSession.DocumentStore;
        }

        public string Generate()
        {
            var id = keyGenerator.GenerateDocumentKey(
                documentStore.DatabaseCommands, documentStore.Conventions, null);
            return id;
        }
    }
}