namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetPracticeWordQueryHandler : GetPracticeWordQueryHandlerBase<GetPracticeWordQuery>
    {
        public override bool CanExecute(GetPracticeWordQuery query, User currentUser)
        {
            return DefaultCanExecute(currentUser, query.PracticeSessionId);
        }

        public override PracticeWordResult Handle(GetPracticeWordQuery query)
        {
            var practiceSession = Session.Load<PracticeSession>(query.PracticeSessionId);
            var practiceWord = practiceSession.GetWordById(query.PracticeWordId);
            var word = Session.Include<Word>(x => x.WordListId).Load<Word>(practiceWord.WordId);
            var wordList = Session.Load<WordList>(word.WordListId);
            var wordFavourite = Session.Load<WordFavourite>(WordFavourite.GetId(word.Id, query.UserId));
            var isFavourite = wordFavourite != null && wordFavourite.IsFavourite;
            var result = new PracticeWordResult(practiceWord, word, practiceSession, wordList, isFavourite);
            return result;
        }
    }
}