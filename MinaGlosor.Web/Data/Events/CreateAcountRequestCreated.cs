using System;

namespace MinaGlosor.Web.Data.Events
{
    public class CreateAcountRequestCreated : IDomainEvent
    {
        public CreateAcountRequestCreated(string email, Guid activationCode)
        {
            ActivationCode = activationCode;
            Email = email;
        }

        public string Email { get; private set; }

        public Guid ActivationCode { get; private set; }
    }
}