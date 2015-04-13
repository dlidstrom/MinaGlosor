using System;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class ResetPasswordRequested
    {
        public ResetPasswordRequested(string email, string activationCode)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (activationCode == null) throw new ArgumentNullException("activationCode");
            Email = email;
            ActivationCode = activationCode;
        }

        public string Email { get; private set; }

        public string ActivationCode { get; private set; }
    }
}