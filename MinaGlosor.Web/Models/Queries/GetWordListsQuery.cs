using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordListsQuery : IQuery<GetWordListsQuery.Result[]>
    {
        private readonly User user;

        public GetWordListsQuery(User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            this.user = user;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public Result[] Execute(IDocumentSession session)
        {
            var wordLists = session.Query<WordListIndex.Result, WordListIndex>()
                                   .Where(x => x.OwnerId == user.Id)
                                   .ProjectFromIndexFieldsInto<WordListIndex.Result>()
                                   .ToArray();
            var result = wordLists.Select(x => new Result(x)).ToArray();
            return result;
        }

        public class Result
        {
            public Result(WordListIndex.Result wordList)
            {
                if (wordList == null) throw new ArgumentNullException("wordList");

                WordListId = WordList.FromId(wordList.Id);
                OwnerId = User.FromId(wordList.OwnerId);
                Name = wordList.Name;
                NumberOfWords = wordList.NumberOfWords;
            }

            public string WordListId { get; private set; }

            public string OwnerId { get; private set; }

            public string Name { get; private set; }

            public int NumberOfWords { get; private set; }
        }
    }
}