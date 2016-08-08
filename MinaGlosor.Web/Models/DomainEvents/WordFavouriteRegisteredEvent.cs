using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordFavouriteRegisteredEvent : ModelEvent
    {
        public WordFavouriteRegisteredEvent(string id, string wordId, string userId, bool isFavourite)
            : base(id)
        {
            WordId = wordId;
            UserId = userId;
            IsFavourite = isFavourite;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private WordFavouriteRegisteredEvent()
#pragma warning restore 612, 618
        {
        }

        public string WordId { get; private set; }

        public string UserId { get; private set; }

        public bool IsFavourite { get; private set; }
    }
}