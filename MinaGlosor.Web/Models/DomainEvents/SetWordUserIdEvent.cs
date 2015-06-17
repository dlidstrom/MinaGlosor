using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class SetWordUserIdEvent : ModelEvent
    {
        public SetWordUserIdEvent(string id, string userId)
            : base(id)
        {
            UserId = userId;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private SetWordUserIdEvent()
#pragma warning restore 612, 618
        {
        }

        public string UserId { get; private set; }
    }
}