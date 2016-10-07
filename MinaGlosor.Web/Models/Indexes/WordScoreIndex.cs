using System.Linq;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class WordScoreIndex : AbstractIndexCreationTask<WordScore>
    {
        public WordScoreIndex()
        {
            Map = wordScores => from wordScore in wordScores
                                select new
                                    {
                                        wordScore.OwnerId,
                                        wordScore.WordId,
                                        wordScore.WordListId,
                                        wordScore.RepeatAfterDate,
                                        wordScore.WordDifficulty
                                    };
        }
    }
}