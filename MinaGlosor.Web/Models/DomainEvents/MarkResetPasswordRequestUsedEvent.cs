using System;
using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class MarkResetPasswordRequestUsedEvent : ModelEvent
    {
        public MarkResetPasswordRequestUsedEvent(string id, DateTime date)
            : base(id)
        {
            Date = date;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private MarkResetPasswordRequestUsedEvent()
#pragma warning restore 612, 618
        {
        }

        public DateTime Date { get; private set; }
    }
}