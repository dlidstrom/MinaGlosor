using System.Linq;
using Raven.Client.Indexes;

namespace MinaGlosor.Web.Models.Indexes
{
    public class WordListIndex : AbstractIndexCreationTask<WordList>
    {
        public WordListIndex()
        {
            Map = wordLists => from wordList in wordLists
                               select new
                               {
                                   wordList.Id,
                                   wordList.OwnerId,
                                   wordList.Name,
                                   wordList.NumberOfWords,
                                   wordList.PublishState
                               };
        }
    }
}