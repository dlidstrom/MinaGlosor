using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class PublishWordListCommand : ICommand<object>
    {
        public PublishWordListCommand(string wordListId, bool publish)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            WordListId = WordList.ToId(wordListId);
            Publish = publish;
        }

        public string WordListId { get; private set; }
        public bool Publish { get; private set; }
    }
}