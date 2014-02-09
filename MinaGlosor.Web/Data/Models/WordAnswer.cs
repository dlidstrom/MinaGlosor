using Raven.Imports.Newtonsoft.Json;

namespace MinaGlosor.Web.Data.Models
{
    public class WordAnswer
    {
        public WordAnswer(string wordId, string wordListId, User user)
        {
            Id = GetId(wordId, user);
            WordListId = wordListId;
            WordId = wordId;
        }

        [JsonConstructor]
        private WordAnswer(string wordId, string wordListId)
        {
            WordId = wordId;
            WordListId = wordListId;
        }

        public string Id { get; set; }

        public string WordId { get; private set; }

        public string WordListId { get; private set; }

        public int Confidence { get; private set; }

        public static string GetId(string wordId, User user)
        {
            return string.Format("wordanswer-{0}-{1}", wordId, user.Id);
        }

        public void AddConfidence(int confidence)
        {
            Confidence += confidence;
        }
    }
}