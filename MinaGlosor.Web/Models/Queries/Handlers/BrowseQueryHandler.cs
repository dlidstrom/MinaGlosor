using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

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
            RavenQueryStatistics stats;
            var wordLists = Session.Query<WordList, WordListIndex>()
                                   .Statistics(out stats)
                                   .OrderBy(x => x.Name)
                                   .Skip((query.Page - 1) * query.ItemsPerPage)
                                   .Take(query.ItemsPerPage)
                                   .ToArray();
            var ownerIds = new HashSet<string>(wordLists.Select(x => x.OwnerId));
            var owners = Session.Load<User>(ownerIds);
            var ownersDict = owners.ToDictionary(x => x.Id);
            return new BrowseQuery.Result(
                wordLists,
                ownersDict,
                stats.TotalResults,
                query.Page,
                query.ItemsPerPage);
        }
    }
}