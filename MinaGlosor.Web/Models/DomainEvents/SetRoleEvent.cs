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

        [JsonConstructor]
        private SetRoleEvent()
        {
        }

        public UserRole Role { get; private set; }
    }
}