using System.Linq;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class ChangeLogEntryIndex : AbstractIndexCreationTask<ChangeLogEntry>
    {
        public ChangeLogEntryIndex()
        {
            Map = entries => from entry in entries
                             select new
                                 {
                                     entry.CreatedDate,
                                     entry.UserId,
                                     entry.CorrelationId
                                 };
        }
    }
}