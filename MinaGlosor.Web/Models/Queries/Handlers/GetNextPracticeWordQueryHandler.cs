using Raven.Client;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetNextPracticeWordQueryHandler : GetPracticeWordQueryHandlerBase<GetNextPracticeWordQuery>
    {
        public override IDocumentSession Session { get; set; }

        public override bool CanExecute(GetNextPracticeWordQuery query, User currentUser)
        {
            return DefaultCanExecute(currentUser, query.PracticeSessionId);
        }

        public override PracticeWordResult Handle(GetNextPracticeWordQuery query)
        {
            var practiceSession = Session.Load<PracticeSession>(query.PracticeSessionId);
            var practiceWord = practiceSession.GetNextWord();
            var word = Session.Include<Word>(x => x.WordListId).Load<Word>(practiceWord.WordId);
            var wordList = Session.Load<WordList>(word.WordListId);
            var wordFavourite = Session.Load<WordFavourite>(WordFavourite.GetId(word.Id, query.UserId));
            var isFavourite = wordFavourite != null && wordFavourite.IsFavourite;
            var result = new PracticeWordResult(practiceWord, word, practiceSession, wordList, isFavourite);
            return result;
        }
    }
}