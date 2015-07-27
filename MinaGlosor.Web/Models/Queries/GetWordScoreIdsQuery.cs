using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordScoreIdsQuery : IQuery<GetWordScoreIdsQuery.Result>
    {
        private readonly string wordId;

        public GetWordScoreIdsQuery(string wordId)
        {
            this.wordId = wordId;
        }

        public bool CanExecute(Raven.Client.IDocumentSession session, User currentUser)
        {
            return true;
        }

        public Result Execute(Raven.Client.IDocumentSession session)
        {
            var query = session.Query<WordScore, WordScoreIndex>().Where(x => x.WordId == wordId).Select(x => x.Id);
            var ids = query.ToArray();
            var result = new Result(ids);
            return result;
        }

        public class Result
        {
            public Result(string[] ids)
            {
                if (ids == null) throw new ArgumentNullException("ids");
                Ids = ids;
            }

            public string[] Ids { get; private set; }
        }
    }
}