using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordQuery : IQuery<GetWordQuery.Result>
    {
        private readonly string wordId;

        public GetWordQuery(string wordId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");

            this.wordId = Word.ToId(wordId);
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public Result Execute(IDocumentSession session)
        {
            var word = session.Load<Word>(wordId);
            return new Result(word);
        }

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