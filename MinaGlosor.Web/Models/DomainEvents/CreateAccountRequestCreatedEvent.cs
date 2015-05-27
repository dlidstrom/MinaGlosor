using System;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class CreateAccountRequestCreatedEvent : ModelEvent
    {
        public CreateAccountRequestCreatedEvent(string email, Guid activationCode)
        {
            if (email == null) throw new ArgumentNullException("email");

            Email = email;
            ActivationCode = activationCode;
        }

        [JsonConstructor]
        public CreateAccountRequestCreatedEvent()
        {
        }

        public string Email { get; private set; }

        public Guid ActivationCode { get; private set; }
    }
}