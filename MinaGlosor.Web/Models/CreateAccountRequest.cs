using System;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Abstractions;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class CreateAccountRequest : DomainModel
    {
        public CreateAccountRequest(string id, string email)
            : base(id)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (email.Length > 254)
                throw new ArgumentException("Email can be at most 254 characters", "email");

            Apply(new CreateAccountRequestCreatedEvent(email, Guid.NewGuid()));
        }

        [JsonConstructor]
        private CreateAccountRequest()
        {
        }

        public string Email { get; private set; }

        public Guid ActivationCode { get; private set; }

        public DateTime? Used { get; private set; }

        public bool HasBeenUsed()
        {
            return Used.HasValue;
        }

        public void MarkAsUsed()
        {
            Apply(new MarkCreateAccountRequestUsedEvent(SystemTime.UtcNow));
        }

        private void ApplyEvent(CreateAccountRequestCreatedEvent @event)
        {
            Email = @event.Email;
            ActivationCode = @event.ActivationCode;
        }

        private void ApplyEvent(MarkCreateAccountRequestUsedEvent @event)
        {
            Used = @event.Date;
        }
    }
}