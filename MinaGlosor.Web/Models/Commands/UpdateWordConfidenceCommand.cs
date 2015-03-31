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
            return new Result(practiceSession.GetStatistics());
        }

        public class Result
        {
            public Result(PracticeSessionStatistics statistics)
            {
                if (statistics == null) throw new ArgumentNullException("statistics");

                IsFinished = statistics.IsFinished;
                Green = statistics.Green;
                Blue = statistics.Blue;
                Yellow = statistics.Yellow;
            }

            public bool IsFinished { get; private set; }

            public int Green { get; private set; }

            public int Blue { get; private set; }

            public int Yellow { get; private set; }
        }
    }
}