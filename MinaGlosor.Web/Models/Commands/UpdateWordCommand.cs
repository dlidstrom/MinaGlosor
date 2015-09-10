using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class UpdateWordCommand : ICommand<object>
    {
        public UpdateWordCommand(string wordId, string text, string definition)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (text == null) throw new ArgumentNullException("text");
            if (definition == null) throw new ArgumentNullException("definition");

            WordId = Word.ToId(wordId);
            Text = text;
            Definition = definition;
        }

        public string WordId { get; private set; }

        public string Text { get; private set; }

        public string Definition { get; private set; }
    }
}