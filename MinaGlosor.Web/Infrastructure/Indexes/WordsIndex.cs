using System.Linq;
using MinaGlosor.Web.Data.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Infrastructure.Indexes
{
    public class WordsIndex : AbstractMultiMapIndexCreationTask<WordsIndex.Result>
    {
        public WordsIndex()
        {
            AddMap<Word>(words => from word in words
                                  select new
                                      {
                                          WordId = word.Id,
                                          word.WordListId,
                                          word.Text,
                                          word.Definition,
                                          EasynessFactor = 0
                                      });

            AddMap<WordAnswer>(answers => from answer in answers
                                          select new
                                              {
                                                  answer.WordId,
                                                  answer.WordListId,
                                                  Text = (string)null,
                                                  Definition = (string)null,
                                                  answer.EasynessFactor
                                              });

            Reduce = words => from word in words
                              group word by new { word.WordId, word.WordListId }
                                  into g
                                  select new Result
                                      {
                                          WordId = g.Key.WordId,
                                          WordListId = g.Key.WordListId,
                                          Text = g.Single(x => x.Text != null).Text,
                                          Definition = g.Single(x => x.Definition != null).Definition,
                                          EasynessFactor = g.Max(x => x.EasynessFactor)
                                      };

            Store(x => x.WordId, FieldStorage.Yes);
            Store(x => x.WordListId, FieldStorage.Yes);
            Store(x => x.Text, FieldStorage.Yes);
            Store(x => x.Definition, FieldStorage.Yes);
            Store(x => x.EasynessFactor, FieldStorage.Yes);
        }

        public class Result
        {
            public string WordId { get; set; }

            public string WordListId { get; set; }

            public string Text { get; set; }

            public string Definition { get; set; }

            public double EasynessFactor { get; set; }
        }
    }
}