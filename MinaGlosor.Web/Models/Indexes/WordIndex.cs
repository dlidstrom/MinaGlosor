using System.Linq;
using Raven.Abstractions.Data;
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

            Indexes.Add(x => x.Text, FieldIndexing.Analyzed);
            Indexes.Add(x => x.Definition, FieldIndexing.Analyzed);
            IndexSuggestions.Add(x => x.Text, new SuggestionOptions { Distance = StringDistanceTypes.Levenshtein, Accuracy = 0.5f });
            IndexSuggestions.Add(x => x.Definition, new SuggestionOptions { Distance = StringDistanceTypes.Levenshtein, Accuracy = 0.5f });
        }
    }
}