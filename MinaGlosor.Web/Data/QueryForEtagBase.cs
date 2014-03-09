namespace MinaGlosor.Web.Data
{
    public abstract class QueryForEtagBase<TResult>
    {
        public string QueryForEtag(IDbContext context)
        {
            //QueryHeaderInformation queryHeaderInformation;
            //var query = GetQuery(session);
            //session.Advanced.Stream(
            //    query,
            //    out queryHeaderInformation);
            //var formattedEtag = string.Format("\"{0}\"", queryHeaderInformation.ResultEtag);
            //return formattedEtag;
            return null;
        }

        protected abstract TResult GetQuery();
    }
}