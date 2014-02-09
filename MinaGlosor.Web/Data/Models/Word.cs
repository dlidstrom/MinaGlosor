using System;

namespace MinaGlosor.Web.Data.Models
{
    public class Word
    {
        public Word(string wordListId, string text, string definition)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (definition == null) throw new ArgumentNullException("definition");
            if (text.Length > 1024)
                throw new ArgumentException("Max 1024 characters", "text");
            if (definition.Length > 1024)
                throw new ArgumentException("Max 1024 characters", "definition");
            WordListId = wordListId;
            Text = text;
            Definition = definition;
        }

        public string Id { get; set; }

        public string WordListId { get; private set; }

        public string Text { get; private set; }

        public string Definition { get; private set; }
    }
}