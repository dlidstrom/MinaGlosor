using System;
using System.ComponentModel.DataAnnotations;
using MinaGlosor.Web.Data.Events;

namespace MinaGlosor.Web.Data.Models
{
    public class CreateAccountRequest
    {
        public CreateAccountRequest(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (email.Length > 320)
                throw new ArgumentException("Email can be at most 320 characters", "email");
            Email = email;
            ActivationCode = Guid.NewGuid();
            DomainEvent.Raise(new CreateAcountRequestCreated(email, ActivationCode));
        }

        private CreateAccountRequest()
        {
        }

        public int Id { get; set; }

        [Required, MaxLength(320)]
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