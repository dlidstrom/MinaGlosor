using System;

namespace MinaGlosor.Web.Models.Queries
{
    public class PracticeWordResult
    {
        public PracticeWordResult(
            PracticeWord practiceWord,
            Word word,
            PracticeSession practiceSession,
            WordList wordList,
            bool isFavourite)
        {
            if (practiceWord == null) throw new ArgumentNullException("practiceWord");
            if (word == null) throw new ArgumentNullException("word");
            if (practiceSession == null) throw new ArgumentNullException("practiceSession");
            if (wordList == null) throw new ArgumentNullException("wordList");

            Text = word.Text;
            Definition = word.Definition;
            PracticeWordId = practiceWord.PracticeWordId;
            PracticeSessionId = PracticeSession.FromId(practiceSession.Id);
            IsFavourite = isFavourite;
            WordId = Word.FromId(word.Id);
            WordListId = WordList.FromId(wordList.Id);
            WordListName = wordList.Name;
            var statistics = practiceSession.GetStatistics();
            Green = statistics.Green;
            Blue = statistics.Blue;
            Yellow = statistics.Yellow;
        }

        public string PracticeSessionId { get; private set; }

        public string Text { get; private set; }

        public string Definition { get; private set; }

        public string PracticeWordId { get; private set; }

        public bool IsFavourite { get; private set; }

        public string WordId { get; private set; }

        public string WordListId { get; private set; }

        public string WordListName { get; private set; }

        public int Green { get; private set; }

        public int Blue { get; private set; }

        public int Yellow { get; private set; }
    }
}