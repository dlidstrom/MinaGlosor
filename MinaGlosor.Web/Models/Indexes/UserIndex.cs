using System.Linq;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class UserIndex : AbstractIndexCreationTask<User>
    {
        public UserIndex()
        {
            Map = users => from user in users
                           select new
                           {
                               user.Email,
                               user.Role,
                               user.Username
                           };
        }
    }
}