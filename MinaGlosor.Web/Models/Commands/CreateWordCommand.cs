using System;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Queries;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateWordCommand : ICommand<string>
    {
        private readonly string text;
        private readonly string definition;
        private readonly string wordListId;
        private readonly Guid correlationId;
        private readonly Guid? causationId;

        public CreateWordCommand(
            string text,
            string definition,
            GetWordListQuery.Result wordListResult,
            Guid correlationId,
            Guid? causationId)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (definition == null) throw new ArgumentNullException("definition");
            if (wordListResult == null) throw new ArgumentNullException("wordListResult");

            this.text = text;
            this.definition = definition;
            this.correlationId = correlationId;
            this.causationId = causationId;
            wordListId = WordList.ToId(wordListResult.WordListId);
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            var wordList = session.Load<WordList>(wordListId);
            var hasAccess = wordList.HasAccess(currentUser.Id);
            return hasAccess;
        }

        public string Execute(IDocumentSession session)
        {
            var generator = new KeyGenerator<Word>(session);
            var id = generator.Generate();
            var word = new Word(id, text, definition, wordListId, correlationId, causationId);
            session.Store(word);
            return Word.FromId(word.Id);
        }
    }
}