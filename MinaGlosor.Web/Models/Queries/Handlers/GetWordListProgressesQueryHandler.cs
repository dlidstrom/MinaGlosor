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
            // todo read all wordlists of the above, construct results using dictionary of word lists
            var result = new GetWordListProgressesQuery.Result(wordListProgresses);
            return result;
        }
    }
}