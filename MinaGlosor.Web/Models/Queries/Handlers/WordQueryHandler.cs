using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class WordQueryHandler :
        IQueryHandler<GetWordsQuery, GetWordsResult>,
        IQueryHandler<GetWordQuery, GetWordQuery.Result>
    {
        public IDocumentSession Session { get; set; }

        public bool CanExecute(GetWordsQuery query, User currentUser)
        {
            return true;
        }

        public GetWordsResult Handle(GetWordsQuery query)
        {
            var wordList = Session.Load<WordList>(query.WordListId);
            RavenQueryStatistics stats;
            var linq = Session.Query<Word, WordIndex>()
                              .Statistics(out stats)
                              .Where(x => x.WordListId == query.WordListId)
                              .Skip((query.Page - 1) * query.ItemsPerPage)
                              .Take(query.ItemsPerPage)
                              .OrderBy(x => x.CreatedDate);
            var words = linq.ToArray();

            var isOwner = query.UserId == wordList.OwnerId;
            var result = new GetWordsResult(
                wordList,
                isOwner,
                isOwner,
                words,
                stats.TotalResults,
                query.Page,
                query.ItemsPerPage);
            return result;
        }

        public bool CanExecute(GetWordQuery query, User currentUser)
        {
            return true;
        }

        public GetWordQuery.Result Handle(GetWordQuery query)
        {
            var word = Session.Load<Word>(query.WordId);
            return new GetWordQuery.Result(word);
        }
    }
}