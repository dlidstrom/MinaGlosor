using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateResetPasswordRequestCommand : ICommand<object>
    {
        private readonly string email;

        public CreateResetPasswordRequestCommand(string email)
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
            var id = KeyGeneratorBase.Generate<ResetPasswordRequest>(session);
            session.Store(new ResetPasswordRequest(id, email));
        }
    }
}