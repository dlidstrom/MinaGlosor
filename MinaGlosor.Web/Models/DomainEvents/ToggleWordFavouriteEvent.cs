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

        [JsonConstructor]
        private ToggleWordFavouriteEvent()
        {
        }

        public bool IsFavourite { get; private set; }
    }
}