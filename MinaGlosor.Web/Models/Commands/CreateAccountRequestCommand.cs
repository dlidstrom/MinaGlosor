using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateAccountRequestCommand : ICommand<string>
    {
        public CreateAccountRequestCommand(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            Email = email;
        }

        public string Email { get; private set; }
    }
}