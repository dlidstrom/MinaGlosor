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

        [JsonConstructor]
        private SetPasswordEvent()
        {
        }

        public string HashedPassword { get; private set; }
    }
}