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
            ActivationCode = Guid.NewGuid().ToString("N");
        }

        [JsonConstructor]
        private CreateAccountRequest(string email, string activationCode)
        {
            Email = email;
            ActivationCode = activationCode;
        }

        public string Email { get; private set; }

        public string ActivationCode { get; private set; }
    }
}