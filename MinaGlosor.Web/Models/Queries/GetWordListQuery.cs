using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordListQuery : IQuery<GetWordListQuery.Result>
    {
        public GetWordListQuery(string wordListId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            this.WordListId = WordList.ToId(wordListId);
        }

        public string WordListId { get; private set; }

        public class Result
        {
            public Result(WordList wordList)
            {
                if (wordList == null) throw new ArgumentNullException("wordList");

                WordListId = WordList.FromId(wordList.Id);
                Name = wordList.Name;
                NumberOfWords = wordList.NumberOfWords;
            }

            public string WordListId { get; private set; }

            public string Name { get; private set; }

            public int NumberOfWords { get; private set; }
        }
    }
}