using System;
using System.Collections.Generic;
using System.Linq;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordsResult
    {
        public GetWordsResult(string wordListName, bool canEdit, bool canAdd, IEnumerable<Word> words, int totalItems, int currentPage, int itemsPerPage)
        {
            if (wordListName == null) throw new ArgumentNullException("wordListName");
            if (words == null) throw new ArgumentNullException("words");
            WordListName = wordListName;
            CanEdit = canEdit;
            CanAdd = canAdd;
            Words = words.Select(x => new WordResult(x)).ToArray();
            Paging = new Paging(totalItems, currentPage, itemsPerPage);
        }

        public string WordListName { get; private set; }

        public bool CanEdit { get; private set; }

        public bool CanAdd { get; private set; }

        public WordResult[] Words { get; private set; }

        public Paging Paging { get; private set; }

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