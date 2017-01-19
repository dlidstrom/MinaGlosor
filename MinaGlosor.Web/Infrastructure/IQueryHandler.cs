using MinaGlosor.Web.Models;
using Raven.Client;

namespace MinaGlosor.Web.Infrastructure
{
    public interface IQueryHandler<in TQuery, out TResult> where TQuery : IQuery<TResult>
    {
        IDocumentSession Session { get; set; }

        bool CanExecute(TQuery query, User currentUser);

        TResult Handle(TQuery query);
    }
}