using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordsResult
    {
        private GetWordsResult(
            string wordListName,
            WordListPublishState? publishState,
            bool canEdit,
            bool canAdd,
            Word[] words,
            int totalItems,
            int currentPage,
            int itemsPerPage)
        {
            if (wordListName == null) throw new ArgumentNullException("wordListName");
            if (words == null) throw new ArgumentNullException("words");
            WordListName = wordListName;
            PublishState = publishState;
            CanEdit = canEdit;
            CanAdd = canAdd;
            Words = words.Select(x => new WordResult(x)).ToArray();
            Paging = new Paging(totalItems, currentPage, itemsPerPage);
        }

        public static GetWordsResult CreateFromWordList(
            WordList wordList,
            bool canEdit,
            bool canAdd,
            Word[] words,
            int totalItems,
            int currentPage,
            int itemsPerPage)
        {
            return new GetWordsResult(
                wordList.Name,
                wordList.PublishState,
                canEdit,
                canAdd,
                words,
                totalItems,
                currentPage,
                itemsPerPage);
        }

        public static GetWordsResult CreateFromFavourites(
            bool canEdit,
            bool canAdd,
            Word[] words,
            int totalResults,
            int currentPage,
            int itemsPerPage)
        {
            return new GetWordsResult(
                string.Empty,
                null,
                canEdit,
                canAdd,
                words,
                totalResults,
                currentPage,
                itemsPerPage);
        }

        public string WordListName { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public WordListPublishState? PublishState { get; private set; }

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