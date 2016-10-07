using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Domain.ProgressModel.Commands
{
    public class UpdateProgressAfterAddWordCommand : ICommand<object>
    {
        public UpdateProgressAfterAddWordCommand(string progressId, int numberOfWords)
        {
            if (progressId == null) throw new ArgumentNullException("progressId");
            ProgressId = progressId;
            NumberOfWords = numberOfWords;
        }

        public string ProgressId { get; private set; }

        public int NumberOfWords { get; private set; }
    }
}