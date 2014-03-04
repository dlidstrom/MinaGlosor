using System;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetUserByEmailQuery : IQuery<User>
    {
        private readonly string email;

        public GetUserByEmailQuery(string email)
        {
            if (email == null) throw new ArgumentNullException("email");
            this.email = email;
        }

        public User Execute(IDbContext session)
        {
            //return session.Query<User, User_ByEmail>().FirstOrDefault(x => x.Email == email);
            return null;
        }
    }
}