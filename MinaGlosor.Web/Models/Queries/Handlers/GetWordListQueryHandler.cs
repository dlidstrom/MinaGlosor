using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetWordListQueryHandler : QueryHandlerBase<GetWordListQuery, GetWordListQuery.Result>
    {
        public override bool CanExecute(User currentUser)
        {
            return true;
        }

        public override GetWordListQuery.Result Handle(GetWordListQuery query)
        {
            var wordList = Session.Load<WordList>(query.WordListId);
            return new GetWordListQuery.Result(wordList);
        }
    }
}