using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateAccountRequestCommand : ICommand
    {
        private readonly string email;

        public CreateAccountRequestCommand(string email)
        {
            if (email == null) throw new ArgumentNullException("email");

            this.email = email;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public void Execute(IDocumentSession session)
        {
            session.Store(new CreateAccountRequest(email));
        }
    }
}