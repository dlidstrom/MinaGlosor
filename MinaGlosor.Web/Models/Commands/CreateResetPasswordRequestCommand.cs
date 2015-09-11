using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateResetPasswordRequestCommand : ICommand<string>
    {
        public CreateResetPasswordRequestCommand(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            Email = email;
        }

        public string Email { get; private set; }

        public void Execute(IDocumentSession session)
        {
        }
    }
}