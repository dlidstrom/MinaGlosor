using System.Linq;
using MinaGlosor.Web.Data.Models;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Infrastructure.Indexes
{
    public class WordIndex : AbstractIndexCreationTask<Word>
    {
        public WordIndex()
        {
            Map = words => from word in words
                           select new
                               {
                                   word.Id,
                                   word.WordListId
                               };
        }
    }
}