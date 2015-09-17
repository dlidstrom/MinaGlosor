using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetUserByEmailQueryHandler : QueryHandlerBase<GetUserByEmailQuery, User>
    {
        public override bool CanExecute(GetUserByEmailQuery query, User currentUser)
        {
            return true;
        }

        public override User Handle(GetUserByEmailQuery query)
        {
            var user = Session.Query<User, UserIndex>().SingleOrDefault(x => x.Email == query.Email);
            return user;
        }
    }
}