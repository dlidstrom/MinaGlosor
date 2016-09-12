using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class UnpublishWordListEvent : ModelEvent
    {
        public UnpublishWordListEvent(string id)
            : base(id)
        {
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private UnpublishWordListEvent()
#pragma warning restore 612, 618
        {
        }
    }
}