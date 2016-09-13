using System;
using MinaGlosor.Web.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordListQuery : IQuery<GetWordListQuery.Result>
    {
        public GetWordListQuery(string wordListId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            WordListId = wordListId;
        }

        public string WordListId { get; private set; }

        public class Result
        {
            public Result(WordList wordList)
            {
                if (wordList == null) throw new ArgumentNullException("wordList");

                WordListId = WordList.FromId(wordList.Id);
                OwnerId = User.FromId(wordList.OwnerId);
                Name = wordList.Name;
                NumberOfWords = wordList.NumberOfWords;
                PublishState = wordList.PublishState;
            }

            public string WordListId { get; private set; }

            public string OwnerId { get; private set; }

            public string Name { get; private set; }

            public int NumberOfWords { get; private set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public WordListPublishState PublishState { get; private set; }
        }
    }
}