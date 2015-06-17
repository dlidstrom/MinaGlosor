using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class SetPasswordEvent : ModelEvent
    {
        public SetPasswordEvent(string id, string hashedPassword)
            : base(id)
        {
            HashedPassword = hashedPassword;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private SetPasswordEvent()
#pragma warning restore 612, 618
        {
        }

        public string HashedPassword { get; private set; }
    }
}