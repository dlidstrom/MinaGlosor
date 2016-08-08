using System.Linq;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordListProgressesQuery : IQuery<GetWordListProgressesQuery.Result>
    {
        public GetWordListProgressesQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; private set; }

        public class Result
        {
            public Result(WordListProgress[] wordListProgresses)
            {
                WordLists = wordListProgresses.Select(x => new WordListProgressResult(x)).ToArray();
            }

            public WordListProgressResult[] WordLists { get; private set; }
        }

        public class WordListProgressResult
        {
            public WordListProgressResult(WordListProgress wordListProgress, WordList wordList)
            {
                WordListId = WordList.FromId(wordListProgress.WordListId);
                OwnerId = User.FromId(wordListProgress.OwnerId);
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