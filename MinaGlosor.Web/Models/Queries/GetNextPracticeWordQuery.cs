using System;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetNextPracticeWordQuery : GetPracticeWordQueryBase
    {
        private readonly string userId;
        private readonly string practiceSessionId;

        public GetNextPracticeWordQuery(string practiceSessionId, string userId)
        {
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
            if (userId == null) throw new ArgumentNullException("userId");

            this.practiceSessionId = PracticeSession.ToId(practiceSessionId);
            this.userId = userId;
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
            var wordFavourite = session.Load<WordFavourite>(WordFavourite.GetId(word.Id, userId));
            var isFavourite = wordFavourite != null && wordFavourite.IsFavourite;
            var result = new PracticeWordResult(practiceWord, word, practiceSession, wordList, isFavourite);
            return result;
        }
    }
}