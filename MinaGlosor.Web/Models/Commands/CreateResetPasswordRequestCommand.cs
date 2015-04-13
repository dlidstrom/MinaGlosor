using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateResetPasswordRequestCommand : ICommand
    {
        private readonly string userId;

        public CreateResetPasswordRequestCommand(string userId)
        {
            if (userId == null) throw new ArgumentNullException("userId");
            this.userId = userId;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public void Execute(IDocumentSession session)
        {
            session.Store(new ResetPasswordRequest(userId));
        }
    }
}