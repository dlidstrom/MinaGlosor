using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MinaGlosor.Web.Data;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Controllers.Api
{
    [CustomAuthorize]
    public abstract class ApiControllerBase : ApiController
    {
        private User currentUser;

        public IDbContext Context { get; set; }

        protected User CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    currentUser = Context.Users
                                         .FirstOrDefault(x => x.Email == User.Identity.Name);
                }

                return currentUser;
            }
        }

        protected Task ExecuteCommandAsync(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            return command.ExecuteAsync(Context);
        }

        protected Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.ExecuteAsync(Context);
        }
    }
}