using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries.Handlers
{
    public class GetWordsQueryHandler : QueryHandlerBase<GetWordsQuery, GetWordsResult>
    {
        public override bool CanExecute(GetWordsQuery query, User currentUser)
        {
            return true;
        }

        public override GetWordsResult Handle(GetWordsQuery query)
        {
            var wordList = Session.Load<WordList>(query.WordListId);
            var linq = from word in Session.Query<Word, WordIndex>()
                       where word.WordListId == query.WordListId
                       orderby word.CreatedDate
                       select word;
            var words = new List<Word>();
            var current = 0;
            while (true)
            {
                var subset = linq.Skip(current).Take(128).ToArray();
                if (subset.Length == 0) break;
                words.AddRange(subset);
                current += subset.Length;
            }

            var result = new GetWordsResult(wordList.Name, words);
            return result;
        }
    }
}