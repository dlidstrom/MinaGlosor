using System;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetNextPracticeWordQuery : GetPracticeWordQueryBase
    {
        private readonly string practiceSessionId;

        public GetNextPracticeWordQuery(string practiceSessionId)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
            this.practiceSessionId = PracticeSession.ToId(practiceSessionId);
        }

        public override bool CanExecute(IDocumentSession session, User currentUser)
        {
            return DefaultCanExecute(session, currentUser, practiceSessionId);
        }

        public override PracticeWordResult Execute(IDocumentSession session)
        {
            var practiceSession = session.Load<PracticeSession>(practiceSessionId);
            var practiceWord = practiceSession.GetNextWord();
            var word = session.Include<Word>(x => x.WordListId).Load<Word>(practiceWord.WordId);
            var wordList = session.Load<WordList>(word.WordListId);
            var result = new PracticeWordResult(practiceWord, word, practiceSession, wordList);
            return result;
        }
    }
}