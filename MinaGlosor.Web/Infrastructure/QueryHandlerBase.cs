using Raven.Client;

namespace MinaGlosor.Web.Infrastructure
{
    public abstract class QueryHandlerBase<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        public IDocumentSession DocumentSession { get; set; }

        public abstract TResult Handle(TQuery query);
    }
}