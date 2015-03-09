using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class UpdateWordCommand : ICommand
    {
        private readonly string wordId;
        private readonly string text;
        private readonly string definition;

        public UpdateWordCommand(string wordId, string text, string definition)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (text == null) throw new ArgumentNullException("text");
            if (definition == null) throw new ArgumentNullException("definition");

            this.wordId = Word.ToId(wordId);
            this.text = text;
            this.definition = definition;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            var word = session.Load<Word>(wordId);
            var wordList = session.Load<WordList>(word.WordListId);
            var hasAccess = wordList.HasAccess(currentUser.Id);
            return hasAccess;
        }

        public void Execute(IDocumentSession session)
        {
            var word = session.Load<Word>(wordId);
            word.Update(text, definition);
        }
    }
}