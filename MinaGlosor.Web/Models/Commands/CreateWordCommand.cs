using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateWordCommand : ICommand<string>
    {
        public CreateWordCommand(
            string text,
            string definition,
            string wordListId)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (definition == null) throw new ArgumentNullException("definition");
            if (wordListId == null) throw new ArgumentNullException("wordListId");

            Text = text;
            Definition = definition;
            WordListId = WordList.ToId(wordListId);
        }

        public string Text { get; private set; }

        public string Definition { get; private set; }

        public string WordListId { get; private set; }
    }
}