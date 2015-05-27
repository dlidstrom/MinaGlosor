using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class ResetPasswordRequestedEvent : ModelEvent
    {
        public ResetPasswordRequestedEvent(string id, string email, string activationCode)
            : base(id)
        {
            Email = email;
            ActivationCode = activationCode;
        }

        [JsonConstructor]
        private ResetPasswordRequestedEvent()
        {
        }

        public string Email { get; private set; }

        public string ActivationCode { get; private set; }
    }
}