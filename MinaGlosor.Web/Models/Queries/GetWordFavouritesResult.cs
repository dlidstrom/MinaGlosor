using System;
using System.Linq;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordFavouritesResult
    {
        public GetWordFavouritesResult(
            Word[] words,
            int totalItems,
            int currentPage,
            int itemsPerPage)
        {
            if (words == null) throw new ArgumentNullException("words");
            Words = words.Select(x => new WordResult(x)).ToArray();
            Paging = new Paging(totalItems, currentPage, itemsPerPage);
        }

        public WordResult[] Words { get; private set; }

        public Paging Paging { get; private set; }

        public class WordResult
        {
            public WordResult(Word word)
            {
                if (word == null) throw new ArgumentNullException("word");

                Id = Word.FromId(word.Id);
                WordListId = WordList.FromId(word.WordListId);
                Text = word.Text;
                Definition = word.Definition;
            }

            public string Id { get; private set; }

            public string WordListId { get; private set; }

            public string Text { get; private set; }

            public string Definition { get; private set; }
        }
    }
}