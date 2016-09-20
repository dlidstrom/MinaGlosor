using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class UpdateWordScoreDifficultyEvent : ModelEvent
    {
        public UpdateWordScoreDifficultyEvent(string id, WordDifficulty wordDifficulty)
            : base(id)
        {
            WordDifficulty = wordDifficulty;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private UpdateWordScoreDifficultyEvent()
#pragma warning restore 612, 618
        {
        }

        public WordDifficulty WordDifficulty { get; private set; }
    }
}