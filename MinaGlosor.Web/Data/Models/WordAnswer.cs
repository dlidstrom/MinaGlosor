using System;

namespace MinaGlosor.Web.Data.Models
{
    public class WordAnswer
    {
        public WordAnswer(Word word, WordList wordList, User user)
        {
            Id = GetId(word, user);
            WordListId = wordList.Id;
            WordId = word.Id;
            EasynessFactor = 0;
        }

        private WordAnswer()
        {
        }

        public double EasynessFactor { get; private set; }

        public string Id { get; set; }

        public int WordId { get; private set; }

        public int WordListId { get; private set; }

        public static string GetId(Word word, User user)
        {
            return string.Format("wordanswer-{0}-{1}", word.Id, user.Id);
        }

        public void UpdateEasynessFactor(int confidence)
        {
            var nextEf = EasynessFactor - 0.8 + 0.28 * confidence - 0.02 * confidence * confidence;
            EasynessFactor = Math.Min(2.5, Math.Max(0, nextEf));
        }
    }
}