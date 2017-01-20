using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using MinaGlosor.Web.Models.Queries;
using Raven.Client;

namespace MinaGlosor.Web.Models.Domain.ProgressModel.Queries.Handler
{
    public class ProgressQueryHandler :
        IQueryHandler<GetProgressQuery, GetProgressQuery.Result>,
        IQueryHandler<GetProgressListQuery, GetProgressListQuery.Result>,
        IQueryHandler<GetProgressListByWordListIdQuery, GetProgressListByWordListIdQuery.Result>
    {
        public IDocumentSession Session { get; set; }

        public bool CanExecute(GetProgressQuery query, User currentUser)
        {
            return true;
        }

        public GetProgressQuery.Result Handle(GetProgressQuery query)
        {
            var progress = Session.Load<Progress>(query.ProgressId);
            if (progress == null) return null;
            var result = new GetProgressQuery.Result(progress);
            return result;
        }

        public bool CanExecute(GetProgressListQuery query, User currentUser)
        {
            return query.UserId == currentUser.Id;
        }

        public GetProgressListQuery.Result Handle(GetProgressListQuery query)
        {
            RavenQueryStatistics stats;
            var progresses = Session.Query<Progress, ProgressIndex>()
                                    .Statistics(out stats)
                                    .Where(x => x.OwnerId == query.UserId)
                                    .OrderBy(x => x.NumberOfWordsSortOrder)
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

        public bool CanExecute(GetProgressListByWordListIdQuery query, User currentUser)
        {
            return true;
        }

        public GetProgressListByWordListIdQuery.Result Handle(GetProgressListByWordListIdQuery query)
        {
            var linq = Session.Query<Progress, ProgressIndex>()
                              .Where(x => x.WordListId == query.WordListId)
                              .Select(x => x.Id);
            var ids = linq.ToArray();
            var result = new GetProgressListByWordListIdQuery.Result(ids);
            return result;
        }
    }
}