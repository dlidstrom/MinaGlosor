using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Queries
{
    public class GetWordsQuery : IQuery<GetWordsResult>
    {
        public GetWordsQuery(string wordListId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            WordListId = WordList.ToId(wordListId);
        }

        public string WordListId { get; private set; }
    }
}