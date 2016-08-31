using System.Linq;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class WordListProgressIndex : AbstractIndexCreationTask<Progress>
    {
        public WordListProgressIndex()
        {
            Map = progresses => from progress in progresses
                                select new
                                {
                                    progress.Id,
                                    progress.OwnerId
                                };
        }
    }
}