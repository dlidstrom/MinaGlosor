using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class UpdateWordConfidenceCommand : ICommand<UpdateWordConfidenceCommand.Result>
    {
        public UpdateWordConfidenceCommand(string practiceSessionId, string practiceWordId, ConfidenceLevel confidenceLevel)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
            if (practiceWordId == null) throw new ArgumentNullException("practiceWordId");

            PracticeSessionId = PracticeSession.ToId(practiceSessionId);
            PracticeWordId = practiceWordId;
            ConfidenceLevel = confidenceLevel;
        }

        public string PracticeSessionId { get; private set; }

        public string PracticeWordId { get; private set; }

        public ConfidenceLevel ConfidenceLevel { get; private set; }

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