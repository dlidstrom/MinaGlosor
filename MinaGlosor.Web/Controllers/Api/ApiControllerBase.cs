using System;
using System.Linq;
using System.Web.Http;
using MinaGlosor.Web.Data;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Infrastructure.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Controllers.Api
{
    [CustomAuthorize]
    public abstract class ApiControllerBase : ApiController
    {
        private User currentUser;

        public IDocumentSession Session { private get; set; }

        public User CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    currentUser = Session.Query<User, User_ByEmail>()
                                         .FirstOrDefault(x => x.Email == User.Identity.Name);
                }

                return currentUser;
            }
        }

        [NonAction]
        public void SaveChanges()
        {
            Session.SaveChanges();
        }

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            command.Execute(Session);
        }

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.Execute(Session);
        }

        protected string ExecuteQueryForEtag<TResult>(QueryForEtagBase<TResult> query)
        {
            return query.QueryForEtag(Session);
        }
    }
}