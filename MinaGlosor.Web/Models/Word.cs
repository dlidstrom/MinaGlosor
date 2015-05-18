using System;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Abstractions;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class Word
    {
        public Word(string id, string text, string definition, string wordListId, Guid correlationId, Guid? causationId)
        {
            if (id == null) throw new ArgumentNullException("id");
            Verify(text, definition, wordListId);
            CreatedDate = SystemTime.UtcNow;
            Text = text;
            Definition = definition;
            WordListId = wordListId;

            DomainEvent.Raise(new WordRegisteredEvent(wordListId, id, correlationId, causationId));
        }

        [JsonConstructor]
        private Word()
        {
        }

        public string Id { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public string Text { get; private set; }

        public string Definition { get; private set; }

        public string WordListId { get; private set; }

        public static string FromId(string wordId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            return wordId.Substring(6);
        }

        public static string ToId(string wordId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            return "Words/" + wordId;
        }

        public static Word CreateFromMigration(
            string id,
            string text,
            string definition,
            DateTime createdDate,
            string wordListId,
            Guid correlationId,
            Guid? causationId)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (definition == null) throw new ArgumentNullException("definition");
            if (wordListId == null) throw new ArgumentNullException("wordListId");

            var word = new Word
                {
                    Id = id,
                    CreatedDate = createdDate,
                    Definition = definition,
                    Text = text,
                    WordListId = wordListId
                };

            DomainEvent.Raise(new WordRegisteredEvent(wordListId, id, correlationId, causationId));
            return word;
        }

        public void Update(string text, string definition)
        {
            Update(text, definition, WordListId);
        }

        public void Update(string text, string definition, string wordListId)
        {
            Verify(text, definition, wordListId);
            Text = text;
            Definition = definition;
            WordListId = wordListId;
        }

        private static void Verify(string text, string definition, string wordListId)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (definition == null) throw new ArgumentNullException("definition");
            if (wordListId == null) throw new ApplicationException("Can only add word to existing word lists");

            if (text.Length > 1024)
                throw new ArgumentOutOfRangeException("text", "Max 1024 characters");
            if (definition.Length > 1024)
                throw new ArgumentOutOfRangeException("definition", "Max 1024 characters");
        }
    }
}