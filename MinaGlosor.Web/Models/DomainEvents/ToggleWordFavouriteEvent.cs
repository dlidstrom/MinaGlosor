using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class ToggleWordFavouriteEvent : ModelEvent
    {
        public ToggleWordFavouriteEvent(string id, bool isFavourite)
            : base(id)
        {
            IsFavourite = isFavourite;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private ToggleWordFavouriteEvent()
#pragma warning restore 612, 618
        {
        }

        public bool IsFavourite { get; private set; }
    }
}