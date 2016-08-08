using System;
using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class WordList : DomainModel
    {
        public WordList(string id, string name, string ownerId)
            : base(id)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (ownerId == null) throw new ArgumentNullException("ownerId");

            if (name.Length > 1024)
            {
                throw new ArgumentOutOfRangeException("name", "Name cannot be longer than 1024 characters");
            }

            Apply(new WordListRegisteredEvent(id, name, ownerId));
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private WordList()
#pragma warning restore 612, 618
        {
        }

        public string Name { get; private set; }

        public string OwnerId { get; private set; }

        public int NumberOfWords { get; private set; }

        public static string FromId(string wordListId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            return wordListId.Substring(10);
        }

        public static string ToId(string wordListId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            return "WordLists/" + wordListId;
        }

        public bool HasAccess(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            return OwnerId == userId;
        }

        public void AddWord()
        {
            Apply(new AddWordEvent(Id, NumberOfWords + 1));
        }

        public WordListProgress CreateProgressForUser(string ownerId)
        {
            var id = string.Format("WordListProgress-{0}-{1}", User.FromId(ownerId), FromId(Id));
            var wordListProgress = new WordListProgress(
                id,
                ownerId,
                Id,
                Name,
                NumberOfWords);
            return wordListProgress;
        }

        private void ApplyEvent(AddWordEvent @event)
        {
            NumberOfWords = @event.NumberOfWords;
        }

        private void ApplyEvent(WordListRegisteredEvent @event)
        {
            Name = @event.Name;
            OwnerId = @event.OwnerId;
        }
    }
}