using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class WordScoreQueryHandler : IQueryHandler<GetWordScoreIdsQuery, GetWordScoreIdsQuery.Result>
    {
        public IDocumentSession Session { get; set; }

        public bool CanExecute(GetWordScoreIdsQuery query, User currentUser)
        {
            return true;
        }

        public GetWordScoreIdsQuery.Result Handle(GetWordScoreIdsQuery query)
        {
            var linq = Session.Query<WordScore, WordScoreIndex>()
                              .Where(x => x.WordId == query.WordId)
                              .Select(x => x.Id);
            var ids = linq.ToArray();
            var result = new GetWordScoreIdsQuery.Result(ids);
            return result;
        }
    }
}