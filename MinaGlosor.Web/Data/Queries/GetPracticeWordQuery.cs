using System;
using System.Linq;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Infrastructure.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetPracticeWordQuery : IQuery<GetPracticeWordQuery.Result>
    {
        private readonly int wordListId;
        private readonly User currentUser;

        public GetPracticeWordQuery(int wordListId, User currentUser)
        {
            if (currentUser == null) throw new ArgumentNullException("currentUser");
            this.wordListId = wordListId;
            this.currentUser = currentUser;
        }

        public Result Execute(IDocumentSession session)
        {
            var first = session.Query<WordPracticeIndex.Result, WordPracticeIndex>()
                               .Where(x => x.WordListId == wordListId)
                               .OrderBy(x => x.Confidence)
                               .First();
            var word = session.Load<Word>(first.WordId);
            return new Result(first, word);
        }

        public class Result
        {
            public Result(WordPracticeIndex.Result result, Word word)
            {
                WordId = result.WordId;
                Text = word.Text;
            }

            public string WordId { get; set; }

            public string Text { get; set; }
        }
    }
}