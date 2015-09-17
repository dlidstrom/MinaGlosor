using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class HasAdminUserQueryHandler : QueryHandlerBase<HasAdminUserQuery, bool>
    {
        public override bool CanExecute(HasAdminUserQuery query, User currentUser)
        {
            return true;
        }

        public override bool Handle(HasAdminUserQuery query)
        {
            var anyAdminUser = Session.Query<User, UserIndex>().Any(x => x.Role == UserRole.Admin);
            return anyAdminUser;
        }
    }
}