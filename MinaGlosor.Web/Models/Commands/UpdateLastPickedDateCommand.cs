using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class UpdateLastPickedDateCommand : ICommand<object>
    {
        private readonly string practiceSessionId;
        private readonly string practiceWordId;

        public UpdateLastPickedDateCommand(string practiceSessionId, string practiceWordId)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
            if (practiceWordId == null) throw new ArgumentNullException("practiceWordId");
            this.practiceSessionId = PracticeSession.ToId(practiceSessionId);
            this.practiceWordId = practiceWordId;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public void Execute(IDocumentSession session)
        {
            var practiceSession = session.Load<PracticeSession>(practiceSessionId);
            practiceSession.UpdateLastPickedDate(practiceWordId);
        }
    }
}