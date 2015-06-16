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

        [JsonConstructor, UsedImplicitly]
#pragma warning disable 618
        private SetWordUserIdEvent()
#pragma warning restore 618
        {
        }

        public string UserId { get; private set; }
    }
}