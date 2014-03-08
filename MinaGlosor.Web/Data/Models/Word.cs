using System;
using System.ComponentModel.DataAnnotations;

namespace MinaGlosor.Web.Data.Models
{
    public class Word
    {
        public Word(WordList wordList, string text, string definition)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (definition == null) throw new ArgumentNullException("definition");
            if (text.Length > 1024)
                throw new ArgumentException("Max 1024 characters", "text");
            if (definition.Length > 1024)
                throw new ArgumentException("Max 1024 characters", "definition");
            WordList = wordList;
            WordListId = wordList.Id;
            Text = text;
            Definition = definition;
        }

        private Word()
        {
        }

        public int Id { get; set; }

        public int WordListId { get; private set; }

        public virtual WordList WordList { get; private set; }

        [Required, MaxLength(1024)]
        public string Text { get; private set; }

        [Required, MaxLength(1024)]
        public string Definition { get; private set; }
    }
}