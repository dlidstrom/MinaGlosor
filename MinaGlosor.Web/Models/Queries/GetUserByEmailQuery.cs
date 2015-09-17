using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetUserByEmailQuery : IQuery<User>
    {
        public GetUserByEmailQuery(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            Email = email;
        }

        public string Email { get; private set; }
    }
}