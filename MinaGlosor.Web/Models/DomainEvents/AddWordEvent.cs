using System;
using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class AddWordEvent : ModelEvent
    {
        public AddWordEvent(string id, int numberOfWords, string ownerId)
            : base(id)
        {
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            NumberOfWords = numberOfWords;
            OwnerId = ownerId;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private AddWordEvent()
#pragma warning restore 612, 618
        {
        }

        public int NumberOfWords { get; private set; }
        public string OwnerId { get; private set; }
    }
}