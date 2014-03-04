using MinaGlosor.Web.Data.Models;
using Raven.Client;

namespace MinaGlosor.Web.Data.Commands
{
    public class CreateWordCommand : ICommand
    {
        private readonly string wordListId;
        private readonly string text;
        private readonly string definition;

        public CreateWordCommand(string wordListId, string text, string definition)
        {
            this.wordListId = wordListId;
            this.text = text;
            this.definition = definition;
        }

        public CreateWordCommand(int wordListId, string text, string definition)
            : this("WordLists-" + wordListId, text, definition)
        {
        }

        public void Execute(IDocumentSession session)
        {
            var wordList = session.Load<WordList>(wordListId);
            session.Store(wordList.AddWord(text, definition));
        }
    }
}