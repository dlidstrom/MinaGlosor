using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordQuery : IQuery<GetWordQuery.Result>
    {
        public GetWordQuery(string wordId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");

            WordId = Word.ToId(wordId);
        }

        public string WordId { get; private set; }

        public class Result
        {
            public Result(Word word)
            {
                if (word == null) throw new ArgumentNullException("word");
                WordId = Word.FromId(word.Id);
                WordListId = WordList.FromId(word.WordListId);
                Text = word.Text;
                Definition = word.Definition;
            }

            public string WordId { get; private set; }

            public string WordListId { get; private set; }

            public string Text { get; private set; }

            public string Definition { get; private set; }
        }
    }
}