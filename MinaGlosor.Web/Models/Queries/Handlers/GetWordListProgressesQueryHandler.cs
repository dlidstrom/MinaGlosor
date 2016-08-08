using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetWordListProgressesQueryHandler : QueryHandlerBase<GetWordListProgressesQuery, GetWordListProgressesQuery.Result>
    {
        public override bool CanExecute(GetWordListProgressesQuery query, User currentUser)
        {
            return query.UserId == currentUser.Id;
        }

        public override GetWordListProgressesQuery.Result Handle(GetWordListProgressesQuery query)
        {
            var wordListProgresses = Session.Query<WordListProgress, WordListProgressIndex>()
                                            .Where(x => x.OwnerId == query.UserId)
                                            .ToArray();
            var wordLists = Session.Load<WordList>(wordListProgresses.Select(x => x.WordListId)).ToDictionary(x => x.Id);
            var wordListProgressResults = wordListProgresses.Select(x => new GetWordListProgressesQuery.WordListProgressResult(x, wordLists[x.WordListId]))
                                                            .ToArray();
            var result = new GetWordListProgressesQuery.Result(wordListProgressResults);
            return result;
        }
    }
}