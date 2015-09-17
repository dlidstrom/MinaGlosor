using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class IsPracticeSessionFinishedQuery : IQuery<bool>
    {
        public IsPracticeSessionFinishedQuery(string practiceSessionId)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
            PracticeSessionId = PracticeSession.ToId(practiceSessionId);
        }

        public string PracticeSessionId { get; private set; }
    }
}