using Raven.Client;

namespace MinaGlosor.Web.Data
{
    public interface ICommand
    {
        void Execute(IDocumentSession session);
    }
}