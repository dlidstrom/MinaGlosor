using System.Linq;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class WordListIndex : AbstractMultiMapIndexCreationTask<WordListIndex.Result>
    {
        public WordListIndex()
        {
            AddMap<WordList>(wordLists => from wordList in wordLists
                                          select new
                                              {
                                                  wordList.Id,
                                                  wordList.OwnerId,
                                                  wordList.Name,
                                                  wordList.NumberOfWords,
                                                  NumberOfWordScores = 0
                                              });

            AddMap<WordScore>(scores => from wordScore in scores
                                        select new
                                            {
                                                Id = wordScore.WordListId,
                                                wordScore.OwnerId,
                                                Name = (string)null,
                                                NumberOfWords = 0,
                                                NumberOfWordScores = 1
                                            });

            Reduce = results => from result in results
                                group result by new { result.Id, result.OwnerId }
                                    into g
                                    select new Result
                                        {
                                            Id = g.Key.Id,
                                            OwnerId = g.Key.OwnerId,
                                            Name = g.First(x => x.Name != null).Name,
                                            NumberOfWords = g.Sum(x => x.NumberOfWords),
                                            NumberOfWordScores = g.Sum(x => x.NumberOfWordScores)
                                        };
        }

        public class Result
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string OwnerId { get; set; }

            public int NumberOfWords { get; set; }

            public int NumberOfWordScores { get; set; }
        }
    }
}