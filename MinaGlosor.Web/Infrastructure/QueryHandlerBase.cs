using MinaGlosor.Web.Models;
using Raven.Client;

namespace MinaGlosor.Web.Infrastructure
{
    public abstract class QueryHandlerBase<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        public IDocumentSession Session { get; set; }

        public abstract bool CanExecute(TQuery query, User currentUser);

        public abstract TResult Handle(TQuery query);
    }
}