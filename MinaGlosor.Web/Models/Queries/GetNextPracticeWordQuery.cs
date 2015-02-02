using System;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetNextPracticeWordQuery : IQuery<GetNextPracticeWordQuery.Result>
    {
        private readonly string practiceSessionId;

        public GetNextPracticeWordQuery(string practiceSessionId)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
            this.practiceSessionId = PracticeSession.ToId(practiceSessionId);
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
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

        public Result Execute(IDocumentSession session)
        {
            var practiceSession = session.Load<PracticeSession>(practiceSessionId);
            var practiceWord = practiceSession.GetNextWord();
            var word = session.Load<Word>(practiceWord.WordId);
            var result = new Result(practiceWord, word);
            return result;
        }

        public class Result
        {
            public Result(PracticeWord practiceWord, Word word)
            {
                if (practiceWord == null) throw new ArgumentNullException("practiceWord");
                if (word == null) throw new ArgumentNullException("word");

                Text = word.Text;
                Definition = word.Definition;
                PracticeWordId = practiceWord.PracticeWordId;
            }

            public string Text { get; private set; }

            public string Definition { get; private set; }

            public string PracticeWordId { get; private set; }
        }
    }
}