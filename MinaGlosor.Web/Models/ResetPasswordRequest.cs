using System;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Abstractions;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class ResetPasswordRequest
    {
        public ResetPasswordRequest(string email)
        {
            if (email == null) throw new ArgumentNullException("email");

            Email = email;
            ActivationCode = Guid.NewGuid().ToString("N");

            DomainEvent.Raise(new ResetPasswordRequested(Email, ActivationCode));
        }

        [JsonConstructor]
        private ResetPasswordRequest()
        {
        }

        public string Email { get; private set; }

        public string ActivationCode { get; private set; }

        public DateTime? UsedDate { get; private set; }

        public bool HasBeenUsed()
        {
            return UsedDate.HasValue;
        }

        public void MarkAsUsed()
        {
            UsedDate = SystemTime.UtcNow;
        }
    }
}