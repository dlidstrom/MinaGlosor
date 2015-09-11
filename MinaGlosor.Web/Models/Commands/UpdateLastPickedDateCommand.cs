using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class UpdateLastPickedDateCommand : ICommand<object>
    {
        public UpdateLastPickedDateCommand(string practiceSessionId, string practiceWordId)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
            if (practiceWordId == null) throw new ArgumentNullException("practiceWordId");
            PracticeSessionId = PracticeSession.ToId(practiceSessionId);
            PracticeWordId = practiceWordId;
        }

        public string PracticeSessionId { get; private set; }

        public string PracticeWordId { get; private set; }
    }
}