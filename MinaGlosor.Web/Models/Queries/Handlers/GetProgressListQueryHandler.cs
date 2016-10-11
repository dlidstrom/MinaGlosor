using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.ProgressModel;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetProgressListQueryHandler : QueryHandlerBase<GetProgressListQuery, GetProgressListQuery.Result>
    {
        public override bool CanExecute(GetProgressListQuery query, User currentUser)
        {
            return query.UserId == currentUser.Id;
        }

        public override GetProgressListQuery.Result Handle(GetProgressListQuery query)
        {
            RavenQueryStatistics stats;
            var progresses = Session.Query<Progress, ProgressIndex>()
                                    .Statistics(out stats)
                                    .Where(x => x.OwnerId == query.UserId)
                                    .Skip((query.Page - 1) * query.ItemsPerPage)
                                    .Take(query.ItemsPerPage)
                                    .ToArray();
            var wordLists = Session.Load<WordList>(progresses.Select(x => x.WordListId))
                                   .ToDictionary(x => x.Id);
            var userIds = new HashSet<string>(progresses.Select(x => x.OwnerId).Concat(wordLists.Values.Select(x => x.OwnerId)));
            var users = Session.Load<User>(userIds)
                               .ToDictionary(x => x.Id);
            var progressResults = new List<GetProgressListQuery.ProgressResult>();
            foreach (var progress in progresses)
            {
                var wordList = wordLists[progress.WordListId];
                var progressOwner = users[progress.OwnerId];
                var wordListOwner = users[wordList.OwnerId];
                var progressResult = new GetProgressListQuery.ProgressResult(progress, wordList, progressOwner, wordListOwner);
                progressResults.Add(progressResult);
            }

            var numberOfFavourites = Session.Query<WordFavourite, WordFavouriteIndex>()
                                            .Where(x => x.UserId == query.UserId)
                                            .Count(x => x.IsFavourite);
            var result = new GetProgressListQuery.Result(
                progressResults.ToArray(),
                numberOfFavourites,
                stats.TotalResults,
                query.Page,
                query.ItemsPerPage);
            return result;
        }
    }
}