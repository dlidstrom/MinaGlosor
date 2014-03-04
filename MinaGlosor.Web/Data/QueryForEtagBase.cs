using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Data
{
    public abstract class QueryForEtagBase<TResult>
    {
        public string QueryForEtag(IDocumentSession session)
        {
            QueryHeaderInformation queryHeaderInformation;
            var query = GetQuery(session);
            session.Advanced.Stream(
                query,
                out queryHeaderInformation);
            var formattedEtag = string.Format("\"{0}\"", queryHeaderInformation.ResultEtag);
            return formattedEtag;
        }

        protected abstract IRavenQueryable<TResult> GetQuery(IDocumentSession session);
    }
}