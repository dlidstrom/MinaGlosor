using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetWordQueryHandler : QueryHandlerBase<GetWordQuery, GetWordQuery.Result>
    {
        public override bool CanExecute(GetWordQuery query, User currentUser)
        {
            return true;
        }

        public override GetWordQuery.Result Handle(GetWordQuery query)
        {
            var word = Session.Load<Word>(query.WordId);
            return new GetWordQuery.Result(word);
        }
    }
}