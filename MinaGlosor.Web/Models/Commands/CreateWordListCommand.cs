using System;
using MinaGlosor.Web.Infrastructure;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateWordListCommand : ICommand<CreateWordListCommand.Result>
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

        public Result Execute(IDocumentSession session)
        {
            var id = KeyGeneratorBase.Generate<WordList>(session);
            var wordList = new WordList(id, name, owner.Id);
            session.Store(wordList);
            return new Result(wordList);
        }

        public class Result
        {
            public Result(WordList wordList)
            {
                if (wordList == null) throw new ArgumentNullException("wordList");

                WordListId = WordList.FromId(wordList.Id);
            }

            public string WordListId { get; private set; }
        }
    }
}