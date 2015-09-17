using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateWordListCommand : ICommand<CreateWordListCommand.Result>
    {
        public CreateWordListCommand(string name, User owner)
        {
            Name = name;
            Owner = owner;
            if (name == null) throw new ArgumentNullException("name");
            if (owner == null) throw new ArgumentNullException("owner");
        }

        public string Name { get; private set; }

        public User Owner { get; private set; }

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