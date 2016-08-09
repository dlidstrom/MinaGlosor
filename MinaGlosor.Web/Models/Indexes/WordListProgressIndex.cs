using System.Linq;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class WordListProgressIndex : AbstractIndexCreationTask<WordListProgress.Model>
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