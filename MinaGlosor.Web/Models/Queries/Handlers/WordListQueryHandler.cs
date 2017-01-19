using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class WordListQueryHandler : IQueryHandler<GetWordListQuery, GetWordListQuery.Result>
    {
        public IDocumentSession Session { get; set; }

        public bool CanExecute(GetWordListQuery query, User currentUser)
        {
            return true;
        }

        public GetWordListQuery.Result Handle(GetWordListQuery query)
        {
            var wordList = Session.Load<WordList>(query.WordListId);
            return new GetWordListQuery.Result(wordList);
        }
    }
}