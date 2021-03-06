using System;
using System.Collections.Generic;
using System.Linq;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordsResult
    {
        public GetWordsResult(string wordListName, IEnumerable<Word> words)
        {
            if (wordListName == null) throw new ArgumentNullException("wordListName");
            if (words == null) throw new ArgumentNullException("words");
            WordListName = wordListName;
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