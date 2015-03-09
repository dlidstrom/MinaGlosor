using System;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class CreateAccountRequestCreated
    {
        public CreateAccountRequestCreated(string email, Guid activationCode)
        {
            if (email == null) throw new ArgumentNullException("email");

            ActivationCode = activationCode;
            Email = email;
        }

        public string Email { get; private set; }

        public Guid ActivationCode { get; private set; }
    }
}