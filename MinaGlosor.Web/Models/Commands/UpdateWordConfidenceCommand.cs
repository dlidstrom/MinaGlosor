using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class UpdateWordConfidenceCommand : ICommand<UpdateWordConfidenceCommand.Result>
    {
        private readonly string practiceSessionId;
        private readonly string practiceWordId;
        private readonly ConfidenceLevel confidenceLevel;

        public UpdateWordConfidenceCommand(string practiceSessionId, string practiceWordId, ConfidenceLevel confidenceLevel)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
            if (practiceWordId == null) throw new ArgumentNullException("practiceWordId");

            this.practiceSessionId = PracticeSession.ToId(practiceSessionId);
            this.practiceWordId = practiceWordId;
            this.confidenceLevel = confidenceLevel;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            var practiceSession = session.Load<PracticeSession>(practiceSessionId);
            var hasAccess = practiceSession.HasAccess(currentUser.Id);
            return hasAccess;
        }

        public Result Execute(IDocumentSession session)
        {
            var practiceSession = session.Load<PracticeSession>(practiceSessionId);
            practiceSession.UpdateConfidence(practiceWordId, confidenceLevel);
            return new Result(practiceSession);
        }

        public class Result
        {
            public Result(PracticeSession practiceSession)
            {
                if (practiceSession == null) throw new ArgumentNullException("practiceSession");

                IsFinished = practiceSession.IsFinished;
            }

            public bool IsFinished { get; private set; }
        }
    }
}