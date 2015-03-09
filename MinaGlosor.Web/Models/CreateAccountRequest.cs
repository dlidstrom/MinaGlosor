using System;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class CreateAccountRequest
    {
        public CreateAccountRequest(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (email.Length > 254)
                throw new ArgumentException("Email can be at most 254 characters", "email");
            Email = email;
            ActivationCode = Guid.NewGuid();

            DomainEvent.Raise(new CreateAccountRequestCreated(email, ActivationCode));
        }

        [JsonConstructor]
        private CreateAccountRequest()
        {
        }

        public string Id { get; set; }

        public string Email { get; private set; }

        public Guid ActivationCode { get; private set; }

        public DateTime? Used { get; private set; }

        public void MarkAsUsed()
        {
            Used = DateTime.Now;
        }

        public bool HasBeenUsed()
        {
            return Used.HasValue;
        }
    }
}