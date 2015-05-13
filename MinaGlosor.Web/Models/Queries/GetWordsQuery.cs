using System;
using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordsQuery : IQuery<GetWordsResult>
    {
        private readonly string wordListId;

        public GetWordsQuery(string wordListId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            this.wordListId = WordList.ToId(wordListId);
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public GetWordsResult Execute(IDocumentSession session)
        {
            var wordList = session.Load<WordList>(wordListId);
            var query = from word in session.Query<Word, WordIndex>()
                        where word.WordListId == wordListId
                        orderby word.CreatedDate
                        select word;
            var words = new List<Word>();
            var current = 0;
            while (true)
            {
                var subset = query.Skip(current).Take(128).ToArray();
                if (subset.Length == 0) break;
                words.AddRange(subset);
                current += subset.Length;
            }

            var result = new GetWordsResult(wordList.Name, words);
            return result;
        }
    }
}