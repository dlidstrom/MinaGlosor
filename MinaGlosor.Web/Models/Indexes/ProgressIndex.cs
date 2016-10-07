using System.Linq;
using MinaGlosor.Web.Models.Domain.ProgressModel;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class ProgressIndex : AbstractIndexCreationTask<Progress>
    {
        public ProgressIndex()
        {
            Map = progresses => from progress in progresses
                                select new
                                {
                                    progress.Id,
                                    progress.OwnerId,
                                    progress.WordListId
                                };
        }
    }
}