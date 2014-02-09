using Raven.Client;

namespace MinaGlosor.Web.Data
{
    public interface IQuery<out TResult>
    {
        TResult Execute(IDocumentSession session);
    }
}