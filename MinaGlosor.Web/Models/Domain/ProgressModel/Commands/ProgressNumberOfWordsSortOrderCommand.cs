using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Domain.ProgressModel.Commands
{
    public class ProgressNumberOfWordsSortOrderCommand : ICommand<object>
    {
        public ProgressNumberOfWordsSortOrderCommand(string wordListId, string ownerId, int numberOfWords)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");

            WordListId = wordListId;
            OwnerId = ownerId;
            NumberOfWords = numberOfWords;
        }

        public string WordListId { get; private set; }
        public string OwnerId { get; private set; }
        public int NumberOfWords { get; private set; }
    }
}