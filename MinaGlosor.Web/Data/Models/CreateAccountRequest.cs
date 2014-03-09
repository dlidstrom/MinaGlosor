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
            Email = email;
            ActivationCode = Guid.NewGuid().ToString("N");
            DomainEvent.Raise(new CreateAcountRequestCreated(email, ActivationCode));
        }

        private CreateAccountRequest()
        {
        }

        public string Email { get; private set; }

        public string ActivationCode { get; private set; }

        public DateTime? Used { get; set; }

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