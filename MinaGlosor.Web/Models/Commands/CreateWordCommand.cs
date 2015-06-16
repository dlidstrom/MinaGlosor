using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateWordCommand : ICommand<string>
    {
        private readonly string text;
        private readonly string definition;
        private readonly string wordListId;

        public CreateWordCommand(
            string text,
            string definition,
            string wordListId)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (definition == null) throw new ArgumentNullException("definition");
            if (wordListId == null) throw new ArgumentNullException("wordListId");

            this.text = text;
            this.definition = definition;
            this.wordListId = WordList.ToId(wordListId);
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            var wordList = session.Load<WordList>(wordListId);
            var hasAccess = wordList.HasAccess(currentUser.Id);
            return hasAccess;
        }

        public string Execute(IDocumentSession session)
        {
            var wordList = session.Load<WordList>(wordListId);
            var word = Word.Create(
                KeyGeneratorBase.Generate<Word>(session),
                text,
                definition,
                wordList);
            session.Store(word);
            return Word.FromId(word.Id);
        }
    }
}