using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordListQuery : IQuery<GetWordListQuery.Result>
    {
        private readonly string wordListId;

        public GetWordListQuery(string wordListId)
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
            return new Result(wordList);
        }

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