using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetUserByUsernameQuery : IQuery<User>
    {
        public GetUserByUsernameQuery(string username)
        {
            if (username == null) throw new ArgumentNullException("username");
            Username = username;
        }

        public string Username { get; private set; }
    }
}