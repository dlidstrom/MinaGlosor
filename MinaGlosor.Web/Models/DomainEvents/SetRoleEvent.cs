using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class SetRoleEvent : ModelEvent
    {
        public SetRoleEvent(string id, UserRole role)
            : base(id)
        {
            Role = role;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private SetRoleEvent()
#pragma warning restore 612, 618
        {
        }

        public UserRole Role { get; private set; }
    }
}