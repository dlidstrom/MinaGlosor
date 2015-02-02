using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class HasAdminUserQuery : IQuery<bool>
    {
        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public bool Execute(IDocumentSession session)
        {
            var adminUser = session.Query<User, UserIndex>().FirstOrDefault(x => x.Role == UserRole.Admin);
            return adminUser != null;
        }
    }
}