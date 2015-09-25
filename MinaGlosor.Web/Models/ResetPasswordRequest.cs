using System;
using JetBrains.Annotations;
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

            var activationCode = Guid.NewGuid().ToString();
            Apply(new ResetPasswordRequestedEvent(id, email, activationCode));
        }

#pragma warning disable 612, 618

        [JsonConstructor, UsedImplicitly]
        private ResetPasswordRequest()
#pragma warning restore 612, 618
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