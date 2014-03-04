using System;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Infrastructure.Indexes;

namespace MinaGlosor.Web.Data.Queries
{
    public class GetPracticeWordQuery : IQuery<GetPracticeWordQuery.Result>
    {
        private static readonly Random Random = new Random();
        private readonly string wordListId;
        private readonly User currentUser;

        public GetPracticeWordQuery(string wordListId, User currentUser)
        {
            if (currentUser == null) throw new ArgumentNullException("currentUser");
            this.wordListId = wordListId;
            this.currentUser = currentUser;
        }

        public Task<Result> ExecuteAsync(IDbContext session)
        {
            return null;
            //var results = session.Query<WordsIndex.Result, WordsIndex>()
            //                     .Where(x => x.WordListId == wordListId)
            //                     .OrderBy(x => x.EasynessFactor)
            //                     .Take(32)
            //                     .ToArray();
            //var randomWord = results[Random.Next(results.Length)];
            //var word = session.Load<Word>(randomWord.WordId);
            //return new Result(randomWord, word);
        }

        public class Result
        {
            public Result(WordsIndex.Result result, Word word)
            {
                WordId = result.WordId;
                Text = word.Text;
            }

            public string WordId { get; private set; }

            public string Text { get; private set; }
        }
    }
}