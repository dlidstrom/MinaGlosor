using System;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetPracticeWordQuery : GetPracticeWordQueryBase
    {
        private readonly string practiceSessionId;
        private readonly string practiceWordId;

        public GetPracticeWordQuery(string practiceSessionId, string practiceWordId)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
            if (practiceWordId == null) throw new ArgumentNullException("practiceWordId");
            this.practiceSessionId = PracticeSession.ToId(practiceSessionId);
            this.practiceWordId = practiceWordId;
        }

        public override bool CanExecute(IDocumentSession session, User currentUser)
        {
            return DefaultCanExecute(session, currentUser, practiceSessionId);
        }

        public override PracticeWordResult Execute(IDocumentSession session)
        {
            var practiceSession = session.Load<PracticeSession>(practiceSessionId);
            var practiceWord = practiceSession.GetWordById(practiceWordId);
            var word = session.Include<Word>(x => x.WordListId).Load<Word>(practiceWord.WordId);
            var wordList = session.Load<WordList>(word.WordListId);
            var result = new PracticeWordResult(practiceWord, word, practiceSession, wordList);
            return result;
        }
    }
}