using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateWordListCommand : ICommand
    {
        private readonly string name;
        private readonly User owner;

        public CreateWordListCommand(string name, User owner)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (owner == null) throw new ArgumentNullException("owner");

            this.name = name;
            this.owner = owner;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public void Execute(IDocumentSession session)
        {
            var wordList = new WordList(name, owner);
            session.Store(wordList);
        }
    }
}