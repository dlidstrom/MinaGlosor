using System.Linq;
using MinaGlosor.Web.Models.Domain.ProgressModel;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class ProgressIndex : AbstractIndexCreationTask<Progress>
    {
        public class Result
        {
            public string OwnerId { get; set; }

            public int NumberOfWords { get; set; }

            public int PercentDone { get; set; }

            public int PercentExpired { get; set; }

            public int Order { get; set; }
        }

        public ProgressIndex()
        {
            Map = progresses => from progress in progresses
                                select new
                                {
                                    progress.Id,
                                    progress.OwnerId,
                                    progress.WordListId,
                                    progress.SortOrder.NumberOfWords,
                                    progress.SortOrder.PercentDone,
                                    progress.SortOrder.PercentExpired,
                                    progress.SortOrder.Order
                                };
        }
    }
}