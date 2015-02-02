using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetUserByEmailQuery : IQuery<User>
    {
        private readonly string email;

        public GetUserByEmailQuery(string email)
        {
            this.email = email;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public User Execute(IDocumentSession session)
        {
            var user = session.Query<User, UserIndex>().SingleOrDefault(x => x.Email == email);
            return user;
        }
    }
}