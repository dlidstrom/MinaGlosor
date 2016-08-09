using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateWordListCommand : ICommand<CreateWordListCommand.Result>
    {
        public CreateWordListCommand(string name, string ownerId)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            Name = name;
            OwnerId = ownerId;
        }

        public string Name { get; private set; }

        public string OwnerId { get; private set; }

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