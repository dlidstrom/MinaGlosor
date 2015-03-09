using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class IsPracticeSessionFinishedQuery : IQuery<bool>
    {
        private readonly string practiceSessionId;

        public IsPracticeSessionFinishedQuery(string practiceSessionId)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
            this.practiceSessionId = PracticeSession.ToId(practiceSessionId);
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            var practiceSession = session.Load<PracticeSession>(practiceSessionId);
            return practiceSession.HasAccess(currentUser.Id);
        }

        public bool Execute(IDocumentSession session)
        {
            var practiceSession = session.Load<PracticeSession>(practiceSessionId);
            return practiceSession.IsFinished;
        }
    }
}