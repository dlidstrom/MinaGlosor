using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public abstract class GetPracticeWordQueryHandlerBase<TQuery> : IQueryHandler<TQuery, PracticeWordResult> where TQuery : IQuery<PracticeWordResult>
    {
        public abstract IDocumentSession Session { get; set; }

        public abstract bool CanExecute(TQuery query, User currentUser);

        public abstract PracticeWordResult Handle(TQuery query);

        protected bool DefaultCanExecute(User currentUser, string practiceSessionId)
        {
            var practiceSession = Session.Load<PracticeSession>(practiceSessionId);
            var hasAccess = practiceSession.HasAccess(currentUser.Id);
            if (hasAccess == false)
            {
                var owner = Session.Load<User>(practiceSession.OwnerId);
                var message = string.Format("Current user={0}, owner={1}", currentUser.Email, owner.Email);
                TracingLogger.Error(
                    EventIds.Error_Permanent_5XXX.Web_GetNextPracticeWord_Unauthorized_5002,
                    message);
            }

            return hasAccess;
        }
    }
}