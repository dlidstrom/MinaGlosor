using System;

namespace MinaGlosor.Web.Models.Queries
{
    public class PracticeWordResult
    {
        public PracticeWordResult(PracticeWord practiceWord, Word word, PracticeSession practiceSessionId, WordList wordList)
        {
            if (practiceWord == null) throw new ArgumentNullException("practiceWord");
            if (word == null) throw new ArgumentNullException("word");
            if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
            if (wordList == null) throw new ArgumentNullException("wordList");

            Text = word.Text;
            Definition = word.Definition;
            PracticeWordId = practiceWord.PracticeWordId;
            PracticeSessionId = PracticeSession.FromId(practiceSessionId.Id);
            WordListId = WordList.FromId(wordList.Id);
            WordListName = wordList.Name;
        }

        public string PracticeSessionId { get; private set; }

        public string Text { get; private set; }

        public string Definition { get; private set; }

        public string PracticeWordId { get; private set; }

        public string WordListId { get; private set; }

        public string WordListName { get; private set; }
    }
}