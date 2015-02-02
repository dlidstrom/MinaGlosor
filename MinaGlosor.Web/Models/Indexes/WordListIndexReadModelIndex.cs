using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class WordListIndex : AbstractIndexCreationTask<WordList, WordListIndex.Result>
    {
        public WordListIndex()
        {
            Map = models => from model in models
                            select new
                                {
                                    model.Id,
                                    model.OwnerId,
                                    model.Name,
                                    model.NumberOfWords
                                };

            Store(x => x.Id, FieldStorage.Yes);
        }

        public class Result
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string OwnerId { get; set; }

            public int NumberOfWords { get; set; }
        }
    }
}