using System;
using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordsQuery : IQuery<GetWordsQuery.Result>
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

        public Result Execute(IDocumentSession session)
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

            var result = new Result(wordList, words);
            return result;
        }

        public class Result
        {
            public Result(WordList wordList, IEnumerable<Word> words)
            {
                if (wordList == null) throw new ArgumentNullException("wordList");
                if (words == null) throw new ArgumentNullException("words");
                WordListName = wordList.Name;
                Words = words.Select(x => new WordResult(x)).ToArray();
            }

            public string WordListName { get; private set; }

            public WordResult[] Words { get; private set; }

            public class WordResult
            {
                public WordResult(Word word)
                {
                    if (word == null) throw new ArgumentNullException("word");

                    Id = Word.FromId(word.Id);
                    Text = word.Text;
                    Definition = word.Definition;
                }

                public string Id { get; private set; }

                public string Text { get; private set; }

                public string Definition { get; private set; }
            }
        }
    }
}