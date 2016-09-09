using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class UpdateWordListNameCommand : ICommand<object>
    {
        public UpdateWordListNameCommand(string wordListId, string wordListName)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (wordListName == null) throw new ArgumentNullException("wordListName");
            WordListName = wordListName;
            WordListId = WordList.ToId(wordListId);
        }

        public string WordListId { get; private set; }
        public string WordListName { get; private set; }
    }
}