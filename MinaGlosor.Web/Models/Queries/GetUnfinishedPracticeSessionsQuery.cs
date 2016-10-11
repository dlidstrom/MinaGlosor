using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetUnfinishedPracticeSessionsQuery : IQuery<GetUnfinishedPracticeSessionsQuery.Result>
    {
        public GetUnfinishedPracticeSessionsQuery(string wordListId, string currentUserId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (currentUserId == null) throw new ArgumentNullException("currentUserId");

            WordListId = WordList.ToId(wordListId);
            CurrentUserId = currentUserId;
        }

        public string WordListId { get; private set; }

        public string CurrentUserId { get; private set; }

        public class Result
        {
            public Result(string wordListName, UnfinishedPracticeSessionResult[] unfinishedPracticeSessionResults)
            {
                if (wordListName == null) throw new ArgumentNullException("wordListName");
                if (unfinishedPracticeSessionResults == null) throw new ArgumentNullException("unfinishedPracticeSessionResults");
                WordListName = wordListName;
                UnfinishedPracticeSessionResults = unfinishedPracticeSessionResults;
            }

            public string WordListName { get; private set; }

            public UnfinishedPracticeSessionResult[] UnfinishedPracticeSessionResults { get; private set; }
        }

        public class UnfinishedPracticeSessionResult
        {
            public UnfinishedPracticeSessionResult(PracticeSession practiceSession)
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