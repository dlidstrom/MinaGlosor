using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class WordIndex : AbstractIndexCreationTask<Word>
    {
        public WordIndex()
        {
            Map = words => from word in words
                           select new
                               {
                                   word.WordListId,
                                   word.CreatedDate,
                                   word.Text,
                                   word.Definition
                               };

            Stores.Add(x => x.Text, FieldStorage.Yes);

            Indexes.Add(x => x.Text, FieldIndexing.Analyzed);
        }
    }
}