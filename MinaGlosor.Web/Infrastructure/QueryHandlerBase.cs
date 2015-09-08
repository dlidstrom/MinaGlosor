using MinaGlosor.Web.Models;
using Raven.Client;

namespace MinaGlosor.Web.Infrastructure
{
    public abstract class QueryHandlerBase<TQuery, TResult> : IQueryHandler where TQuery : IQuery<TResult>
    {
        public IDocumentSession DocumentSession { get; set; }

        public abstract bool CanExecute(User currentUser);

        public abstract TResult Handle(TQuery query);
    }
}