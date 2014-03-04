using System;
using System.Linq;
using System.Web.Http;
using MinaGlosor.Web.Data;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Controllers.Api
{
    [CustomAuthorize]
    public abstract class ApiControllerBase : ApiController
    {
        private User currentUser;

        //public IDocumentSession Session { private get; set; }

        public User CurrentUser
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

        public IDbContext Context { get; set; }

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            command.Execute(Context);
        }

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.Execute(Context);
        }

        protected string ExecuteQueryForEtag<TResult>(QueryForEtagBase<TResult> query)
        {
            return query.QueryForEtag(Context);
        }
    }
}