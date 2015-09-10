using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetPracticeWordQuery : IQuery<PracticeWordResult>
    {
        public GetPracticeWordQuery(string practiceSessionId, string practiceWordId, string userId)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
            if (practiceWordId == null) throw new ArgumentNullException("practiceWordId");
            if (userId == null) throw new ArgumentNullException("userId");

            PracticeSessionId = PracticeSession.ToId(practiceSessionId);
            PracticeWordId = practiceWordId;
            UserId = userId;
        }

        public string PracticeSessionId { get; private set; }

        public string PracticeWordId { get; private set; }

        public string UserId { get; private set; }
    }
}