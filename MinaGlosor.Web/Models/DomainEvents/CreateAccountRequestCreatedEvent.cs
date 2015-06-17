using System;
using JetBrains.Annotations;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class CreateAccountRequestCreatedEvent : ModelEvent
    {
        public CreateAccountRequestCreatedEvent(string id, string email, Guid activationCode)
            : base(id)
        {
            if (email == null) throw new ArgumentNullException("email");

            Email = email;
            ActivationCode = activationCode;
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private CreateAccountRequestCreatedEvent()
#pragma warning restore 612, 618
        {
        }

        public string Email { get; private set; }

        public Guid ActivationCode { get; private set; }
    }
}