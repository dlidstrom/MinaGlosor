using System.Linq;
using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class WordIndex : AbstractIndexCreationTask<Word>
    {
        private const float Accuracy = 0.5f;

        private readonly SuggestionOptions suggestionOptions = new SuggestionOptions
            {
                Distance = StringDistanceTypes.Levenshtein,
                Accuracy = Accuracy
            };

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
            IndexSuggestions.Add(x => x.Text, suggestionOptions);
            IndexSuggestions.Add(x => x.Definition, suggestionOptions);
        }
    }
}