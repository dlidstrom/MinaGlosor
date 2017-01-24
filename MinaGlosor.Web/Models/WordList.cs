using System;
using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class WordList : DomainModel
    {
        private const string NameMaxLength = 1024;

        public WordList(string id, string name, string ownerId)
            : base(id)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (ownerId == null) throw new ArgumentNullException("ownerId");

            if (name.Length > NameMaxLength)
            {
                throw new ArgumentOutOfRangeException("name", string.Format("Name cannot be longer than {0} characters", NameMaxLength));
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

        public WordListPublishState PublishState { get; private set; }

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
            Apply(new AddWordEvent(Id, NumberOfWords + 1, OwnerId));
        }

        public void UpdateName(string wordListName)
        {
            if (wordListName == null) throw new ArgumentNullException("wordListName");
            if (wordListName.Length > NameMaxLength)
            {
                throw new ArgumentOutOfRangeException("wordListName", string.Format("Max {0} characters", NameMaxLength));
            }

            Apply(new UpdateWordListNameEvent(Id, wordListName));
        }

        public void Publish()
        {
            Apply(new PublishWordListEvent(Id, WordListPublishState.Published));
        }

        public void Unpublish()
        {
            Apply(new PublishWordListEvent(Id, WordListPublishState.Private));
        }

        public bool IsPublished()
        {
            return PublishState == WordListPublishState.Published;
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

        private void ApplyEvent(UpdateWordListNameEvent @event)
        {
            Name = @event.WordListName;
        }

        private void ApplyEvent(PublishWordListEvent @event)
        {
            PublishState = @event.WordListPublishState;
        }
    }
}