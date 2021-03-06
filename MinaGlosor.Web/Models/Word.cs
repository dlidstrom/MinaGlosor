﻿using System;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class Word : DomainModel
    {
        private Word(string id, string text, string definition, string userId, string wordListId)
            : base(id)
        {
            Verify(text, definition, userId, wordListId);
            Apply(new WordRegisteredEvent(userId, wordListId, id, text, definition));
        }

#pragma warning disable 612, 618

        [JsonConstructor]
        private Word()
#pragma warning restore 612, 618
        {
        }

        public DateTime CreatedDate { get; private set; }

        public string Text { get; private set; }

        public string Definition { get; private set; }

        public string WordListId { get; private set; }

        public string UserId { get; private set; }

        public static string FromId(string wordId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            return wordId.Substring(6);
        }

        public static string ToId(string wordId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            return "words/" + wordId;
        }

        public static Word Create(string id, string text, string definition, WordList wordList)
        {
            if (id == null) throw new ArgumentNullException("id");
            if (text == null) throw new ArgumentNullException("text");
            if (definition == null) throw new ArgumentNullException("definition");
            if (wordList == null) throw new ArgumentNullException("wordList");
            return new Word(id, text, definition, wordList.OwnerId, wordList.Id);
        }

        public static Word CreateFromMigration(
            string id,
            string text,
            string definition,
            DateTime createdDate,
            string userId,
            string wordListId)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (definition == null) throw new ArgumentNullException("definition");
            if (userId == null) throw new ArgumentNullException("userId");
            if (wordListId == null) throw new ArgumentNullException("wordListId");

            var word = new Word();
            word.Apply(new WordRegisteredEvent(userId, wordListId, id, text, definition));
            word.Apply(new SetCreatedDateFromMigrationEvent(id, createdDate));
            return word;
        }

        public void Update(string text, string definition)
        {
            Update(text, definition, WordListId);
        }

        public void Update(string text, string definition, string wordListId)
        {
            Verify(text, definition, UserId, wordListId);
            Apply(new WordUpdatedEvent(Id, text, definition, wordListId));
        }

        public void SetUserId(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            Apply(new SetWordUserIdEvent(Id, userId));
        }

        private static void Verify(string text, string definition, string userId, string wordListId)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (definition == null) throw new ArgumentNullException("definition");
            if (userId == null) throw new ArgumentNullException("userId");
            if (wordListId == null) throw new ApplicationException("Can only add word to existing word lists");

            if (text.Length > 1024)
                throw new ArgumentOutOfRangeException("text", "Max 1024 characters");
            if (definition.Length > 1024)
                throw new ArgumentOutOfRangeException("definition", "Max 1024 characters");
        }

        private void ApplyEvent(WordUpdatedEvent @event)
        {
            Text = @event.Text;
            Definition = @event.Definition;
            WordListId = @event.WordListId;
        }

        private void ApplyEvent(WordRegisteredEvent @event)
        {
            CreatedDate = @event.CreatedDateTime;
            Text = @event.Text;
            Definition = @event.Definition;
            UserId = @event.UserId;
            WordListId = @event.WordListId;
        }

        private void ApplyEvent(SetCreatedDateFromMigrationEvent @event)
        {
            CreatedDate = @event.CreatedDateFromMigration;
        }

        private void ApplyEvent(SetWordUserIdEvent @event)
        {
            UserId = @event.UserId;
        }
    }
}