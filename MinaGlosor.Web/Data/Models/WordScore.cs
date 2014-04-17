using System;

namespace MinaGlosor.Web.Data.Models
{
    public class WordScore
    {
        public WordScore(User user, Word word)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (word == null) throw new ArgumentNullException("word");

            UserId = user.Id;
            User = user;
            WordId = word.Id;
            Word = word;
        }

        public int Id { get; set; }

        public int UserId { get; private set; }

        public virtual User User { get; private set; }

        public int WordId { get; private set; }

        public virtual Word Word { get; private set; }

        public double EasynessFactor { get; private set; }

        public void UpdateEasynessFactor(int confidence)
        {
            var nextEf = EasynessFactor - 0.8 + 0.28 * confidence - 0.02 * confidence * confidence;
            EasynessFactor = Math.Min(2.5, Math.Max(0, nextEf));
        }
    }
}