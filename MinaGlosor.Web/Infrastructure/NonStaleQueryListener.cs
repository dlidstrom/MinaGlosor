using Raven.Client;
using Raven.Client.Listeners;

namespace MinaGlosor.Web.Infrastructure
{
    public class NonStaleQueryListener : IDocumentQueryListener
    {
        public void BeforeQueryExecuted(IDocumentQueryCustomization customization)
        {
            customization.WaitForNonStaleResultsAsOfLastWrite();
        }
    }
}