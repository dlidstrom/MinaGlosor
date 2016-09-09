using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class UpdateWordListNameEvent : ModelEvent
    {
        public UpdateWordListNameEvent(string id, string wordListName)
            : base(id)
        {
            WordListName = wordListName;
        }

        public string WordListName { get; private set; }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private UpdateWordListNameEvent()
#pragma warning restore 612, 618
        {
        }
    }
}