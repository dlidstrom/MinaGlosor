using JetBrains.Annotations;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.Domain.WordListProgressModel.Events
{
    public class WordIsUpToDateEvent : ModelEvent
    {
        public WordIsUpToDateEvent(string modelId, int newNumberOfWordsExpired, int percentExpired)
            : base(modelId)
        {
            NewNumberOfWordsExpired = newNumberOfWordsExpired;
            PercentExpired = percentExpired;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private WordIsUpToDateEvent()
#pragma warning restore 612, 618
        {
        }

        public int NewNumberOfWordsExpired { get; private set; }

        public int PercentExpired { get; private set; }
    }
}