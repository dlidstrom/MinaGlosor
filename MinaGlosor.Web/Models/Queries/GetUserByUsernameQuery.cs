using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetUserByUsernameQuery : IQuery<User>
    {
        private readonly string username;

        public GetUserByUsernameQuery(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            this.username = username;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public User Execute(IDocumentSession session)
        {
            var existingUser = session.Query<User, UserIndex>().SingleOrDefault(x => x.Username == username);
            return existingUser;
        }
    }
}