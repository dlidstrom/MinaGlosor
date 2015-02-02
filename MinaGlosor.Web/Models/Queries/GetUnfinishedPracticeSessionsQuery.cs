using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetUnfinishedPracticeSessionsQuery : IQuery<GetUnfinishedPracticeSessionsQuery.Result[]>
    {
        private readonly string wordListId;
        private readonly string currentUserId;

        public GetUnfinishedPracticeSessionsQuery(string wordListId, string currentUserId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (currentUserId == null) throw new ArgumentNullException("currentUserId");

            this.wordListId = WordList.ToId(wordListId);
            this.currentUserId = currentUserId;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            var wordList = session.Load<WordList>(wordListId);
            var hasAccess = wordList.HasAccess(currentUser.Id);
            return hasAccess;
        }

        public Result[] Execute(IDocumentSession session)
        {
            var query = from practiceSession in session.Query<PracticeSession, PracticeSessionIndex>()
                        where practiceSession.WordListId == wordListId
                            && practiceSession.IsFinished == false
                            && practiceSession.OwnerId == currentUserId
                        select practiceSession;
            var unfinishedPracticeSessions = query.ToArray();
            var result = unfinishedPracticeSessions.Select(x => new Result(x)).ToArray();
            return result;
        }

        public class Result
        {
            public Result(PracticeSession practiceSession)
            {
                if (practiceSession == null) throw new ArgumentNullException("practiceSession");

                PracticeSessionId = PracticeSession.FromId(practiceSession.Id);
                CreatedDate = practiceSession.CreatedDate;
            }

            public string PracticeSessionId { get; private set; }

            public DateTime CreatedDate { get; private set; }
        }
    }
}