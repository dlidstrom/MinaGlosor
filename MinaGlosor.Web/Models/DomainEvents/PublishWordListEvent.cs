using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class PublishWordListEvent : ModelEvent
    {
        public PublishWordListEvent(string id)
            : base(id)
        {
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private PublishWordListEvent()
#pragma warning restore 612, 618
        {
        }
    }
}