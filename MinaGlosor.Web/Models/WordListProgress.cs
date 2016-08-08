using System;
using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class WordListProgress : DomainModel
    {
        public WordListProgress(
            string id,
            string ownerId,
            string wordListId)
            : base(id)
        {
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            Apply(new WordListProgressCeatedEvent(id, ownerId, wordListId));
        }

        public string OwnerId { get; private set; }

        public string WordListId { get; private set; }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private WordListProgress()
#pragma warning restore 612, 618
        {
        }

        private void ApplyEvent(WordListProgressCeatedEvent @event)
        {
            OwnerId = @event.OwnerId;
            WordListId = @event.WordListId;
        }
    }
}