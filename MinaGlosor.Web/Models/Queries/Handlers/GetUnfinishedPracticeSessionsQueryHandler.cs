using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetUnfinishedPracticeSessionsQueryHandler : QueryHandlerBase<GetUnfinishedPracticeSessionsQuery, GetUnfinishedPracticeSessionsQuery.Result[]>
    {
        public override bool CanExecute(GetUnfinishedPracticeSessionsQuery query, User currentUser)
        {
            // TODO: Check if word list is published?
            return true;
        }

        public override GetUnfinishedPracticeSessionsQuery.Result[] Handle(GetUnfinishedPracticeSessionsQuery query)
        {
            var linq = from practiceSession in Session.Query<PracticeSession, PracticeSessionIndex>()
                       where practiceSession.WordListId == query.WordListId
                             && practiceSession.IsFinished == false
                             && practiceSession.OwnerId == query.CurrentUserId
                       select practiceSession;
            var unfinishedPracticeSessions = linq.ToArray();
            var result = unfinishedPracticeSessions.Select(x => new GetUnfinishedPracticeSessionsQuery.Result(x)).ToArray();
            return result;
        }
    }
}