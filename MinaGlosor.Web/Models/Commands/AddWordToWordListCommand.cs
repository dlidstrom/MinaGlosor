using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class AddWordToWordListCommand : ICommand<object>
    {
        private readonly string wordListId;

        public AddWordToWordListCommand(string wordListId)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            this.wordListId = wordListId;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            var wordList = session.Load<WordList>(wordListId);
            var hasAccess = wordList.HasAccess(currentUser.Id);
            return hasAccess;
        }

        public void Execute(IDocumentSession session)
        {
            var wordList = session.Load<WordList>(wordListId);
            wordList.AddWord();
        }
    }
}