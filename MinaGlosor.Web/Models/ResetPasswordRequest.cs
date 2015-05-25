using System;
using MinaGlosor.Web.Models.DomainEvents;
using Raven.Abstractions;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Models
{
    public class ResetPasswordRequest : DomainModel
    {
        public ResetPasswordRequest(string id, string email)
            : base(id)
        {
            if (email == null) throw new ArgumentNullException("email");

            Apply(new ResetPasswordRequestedEvent(id, Email, ActivationCode));
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
            Apply(new MarkResetPasswordRequestUsedEvent(Id, SystemTime.UtcNow));
        }

        private void ApplyEvent(ResetPasswordRequestedEvent @event)
        {
            Email = @event.Email;
            ActivationCode = @event.ActivationCode;
        }

        private void ApplyEvent(MarkResetPasswordRequestUsedEvent @event)
        {
            UsedDate = @event.Date;
        }
    }
}