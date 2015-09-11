using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetUserByUsernameQueryHandler : QueryHandlerBase<GetUserByUsernameQuery, User>
    {
        public override bool CanExecute(GetUserByUsernameQuery query, User currentUser)
        {
            return true;
        }

        public override User Handle(GetUserByUsernameQuery query)
        {
            var existingUser = Session.Query<User, UserIndex>().SingleOrDefault(x => x.Username == query.Username);
            return existingUser;
        }
    }
}