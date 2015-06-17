using JetBrains.Annotations;
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

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private ResetPasswordRequestedEvent()
#pragma warning restore 612, 618
        {
        }

        public string Email { get; private set; }

        public string ActivationCode { get; private set; }
    }
}