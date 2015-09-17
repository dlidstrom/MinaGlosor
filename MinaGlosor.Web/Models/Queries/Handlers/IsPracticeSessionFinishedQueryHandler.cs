using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class IsPracticeSessionFinishedQueryHandler : QueryHandlerBase<IsPracticeSessionFinishedQuery, bool>
    {
        public override bool CanExecute(IsPracticeSessionFinishedQuery query, User currentUser)
        {
            var practiceSession = Session.Load<PracticeSession>(query.PracticeSessionId);
            return practiceSession.HasAccess(currentUser.Id);
        }

        public override bool Handle(IsPracticeSessionFinishedQuery query)
        {
            var practiceSession = Session.Load<PracticeSession>(query.PracticeSessionId);
            return practiceSession.IsFinished;
        }
    }
}