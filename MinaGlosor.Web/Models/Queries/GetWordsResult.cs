using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordsResult
    {
        public GetWordsResult(
            WordList wordList,
            bool canEdit,
            bool canAdd,
            Word[] words,
            int totalItems,
            int currentPage,
            int itemsPerPage)
        {
            if (wordList == null) throw new ArgumentNullException("wordList");
            if (words == null) throw new ArgumentNullException("words");
            WordListId = WordList.FromId(wordList.Id);
            WordListName = wordList.Name;
            PublishState = wordList.PublishState;
            CanEdit = canEdit;
            CanAdd = canAdd;
            Words = words.Select(x => new WordResult(x)).ToArray();
            Paging = new Paging(totalItems, currentPage, itemsPerPage);
        }

        public string WordListId { get; private set; }

        public string WordListName { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public WordListPublishState PublishState { get; private set; }

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