using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public abstract class GetPracticeWordQueryBase : IQuery<PracticeWordResult>
    {
        public abstract bool CanExecute(IDocumentSession session, User currentUser);

        public abstract PracticeWordResult Execute(IDocumentSession session);

        protected static bool DefaultCanExecute(IDocumentSession session, User currentUser, string practiceSessionId)
        {
            var practiceSession = session.Load<PracticeSession>(practiceSessionId);
            var hasAccess = practiceSession.HasAccess(currentUser.Id);
            if (hasAccess == false)
            {
                var owner = session.Load<User>(practiceSession.OwnerId);
                var message = string.Format("Current user={0}, owner={1}", currentUser.Email, owner.Email);
                TracingLogger.Error(
                    EventIds.Error_Permanent_5XXX.Web_GetNextPracticeWord_Unauthorized_5002,
                    message);
            }

            return hasAccess;
        }
    }
}