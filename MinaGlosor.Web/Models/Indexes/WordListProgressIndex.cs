using System.Linq;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class WordListProgressIndex : AbstractIndexCreationTask<WordListProgress>
    {
        public WordListProgressIndex()
        {
            Map = wordListProgresses => from wordListProgress in wordListProgresses
                                        select new
                                        {
                                            wordListProgress.Id,
                                            wordListProgress.OwnerId
                                        };
        }
    }
}