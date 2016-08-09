using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordExpiredEvent : ModelEvent
    {
        public WordExpiredEvent(string id)
            : base(id)
        {
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private WordExpiredEvent()
#pragma warning restore 612, 618
        {
        }
    }
}