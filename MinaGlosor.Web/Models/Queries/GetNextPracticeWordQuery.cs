using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetNextPracticeWordQuery : IQuery<PracticeWordResult>
    {
        public GetNextPracticeWordQuery(string practiceSessionId, string userId)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
            if (userId == null) throw new ArgumentNullException("userId");

            PracticeSessionId = PracticeSession.ToId(practiceSessionId);
            UserId = userId;
        }

        public string PracticeSessionId { get; private set; }

        public string UserId { get; private set; }
    }
}