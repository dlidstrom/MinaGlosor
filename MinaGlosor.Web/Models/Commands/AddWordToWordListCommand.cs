using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class AddWordToWordListCommand : ICommand<object>
    {
        public AddWordToWordListCommand(string wordListId)
        {
            WordListId = wordListId;
            if (wordListId == null) throw new ArgumentNullException("wordListId");
        }

        public string WordListId { get; private set; }
    }
}