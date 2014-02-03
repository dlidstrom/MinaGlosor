using System.Linq;
using MinaGlosor.Web.Data.Models;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Infrastructure.Indexes
{
    public class WordListIndex : AbstractIndexCreationTask<WordList>
    {
        public WordListIndex()
        {
            Map = items => from item in items
                           select new
                               {
                                   item.Name,
                                   item.OwnerId
                               };
        }
    }
}