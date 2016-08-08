using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetWordListProgressQueryHandler : QueryHandlerBase<GetWordListProgressesQuery, GetWordListProgressesQuery.Result>
    {
        public override bool CanExecute(GetWordListProgressesQuery query, User currentUser)
        {
            throw new System.NotImplementedException();
        }

        public override GetWordListProgressesQuery.Result Handle(GetWordListProgressesQuery query)
        {
            throw new System.NotImplementedException();
        }
    }
}