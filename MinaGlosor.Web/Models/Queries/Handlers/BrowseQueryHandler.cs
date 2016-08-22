using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class BrowseQueryHandler : QueryHandlerBase<BrowseQuery, BrowseQuery.Result>
    {
        public override bool CanExecute(BrowseQuery query, User currentUser)
        {
            return true;
        }

        public override BrowseQuery.Result Handle(BrowseQuery query)
        {
            var wordLists = Session.Query<WordList, WordListIndex>().ToArray();
            return new BrowseQuery.Result(wordLists);
        }
    }
}