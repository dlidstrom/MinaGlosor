using System;
using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class CheckIfWordExpiresEvent : ModelEvent
    {
        public CheckIfWordExpiresEvent(string id, DateTime repeatAfterDate)
            : base(id)
        {
            RepeatAfterDate = repeatAfterDate;
        }

#pragma warning disable 612, 618
        [JsonConstructor, UsedImplicitly]
        private CheckIfWordExpiresEvent()
#pragma warning restore 612, 618
        {
        }

        public DateTime RepeatAfterDate { get; private set; }
    }
}