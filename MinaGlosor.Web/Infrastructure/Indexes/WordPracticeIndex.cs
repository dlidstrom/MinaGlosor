using System.Linq;
using MinaGlosor.Web.Data.Models;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Infrastructure.Indexes
{
    public class WordPracticeIndex : AbstractMultiMapIndexCreationTask<WordPracticeIndex.Result>
    {
        public WordPracticeIndex()
        {
            AddMap<Word>(words => from word in words
                                  select new
                                      {
                                          WordId = word.Id,
                                          word.WordListId,
                                          Confidence = 0
                                      });

            AddMap<WordAnswer>(answers => from answer in answers
                                          select new
                                              {
                                                  answer.WordId,
                                                  answer.WordListId,
                                                  answer.Confidence
                                              });

            Reduce = words => from word in words
                              group word by new { word.WordId, word.WordListId }
                                  into g
                                  select new Result
                                      {
                                          WordId = g.Key.WordId,
                                          WordListId = g.Key.WordListId,
                                          Confidence = g.Sum(x => x.Confidence)
                                      };
        }

        public class Result
        {
            public string WordId { get; set; }

            public int WordListId { get; set; }

            public int Confidence { get; set; }
        }
    }
}