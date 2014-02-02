using System;
using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Data.Models
{
    public class CreateAccountRequest
    {
        public CreateAccountRequest(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            Email = email;
            ActivationCode = Guid.NewGuid();
        }

        [JsonConstructor]
        private CreateAccountRequest(string email, Guid activationCode)
        {
            Email = email;
            ActivationCode = activationCode;
        }

        public string Email { get; private set; }

        public Guid ActivationCode { get; private set; }
    }
}