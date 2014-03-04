using System.Linq;
using MinaGlosor.Web.Data.Models;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Infrastructure.Indexes
{
    public class WordListIndex : AbstractMultiMapIndexCreationTask<WordListIndex.Result>
    {
        public WordListIndex()
        {
            AddMap<Word>(words => from word in words
                                  let wordList = LoadDocument<WordList>(word.WordListId)
                                  select new
                                  {
                                      word.WordListId,
                                      wordList.OwnerId,
                                      WordListName = wordList.Name,
                                      WordCount = 1
                                  });

            AddMap<WordList>(lists => from list in lists
                                      select new
                                      {
                                          WordListId = list.Id,
                                          list.OwnerId,
                                          WordListName = list.Name,
                                          WordCount = 0
                                      });

            Reduce = items => from item in items
                              group item by new { item.OwnerId, item.WordListId, item.WordListName } into g
                              select new Result
                              {
                                  WordListId = g.Key.WordListId,
                                  OwnerId = g.Key.OwnerId,
                                  WordListName = g.Key.WordListName,
                                  WordCount = g.Sum(x => x.WordCount)
                              };
        }

        public class Result
        {
            public string WordListId { get; set; }

            public string WordListName { get; set; }

            public string OwnerId { get; set; }

            public int WordCount { get; set; }
        }
    }
}