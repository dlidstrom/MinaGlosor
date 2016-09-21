using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class UpdateWordScoreDifficultyCommand : ICommand<object>
    {
        public UpdateWordScoreDifficultyCommand(ConfidenceLevel[] confidenceLevels, string wordId, string ownerId)
        {
            if (confidenceLevels == null) throw new ArgumentNullException("confidenceLevels");
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            ConfidenceLevels = confidenceLevels;
            WordId = wordId;
            OwnerId = ownerId;
        }

        public ConfidenceLevel[] ConfidenceLevels { get; private set; }

        public string WordId { get; private set; }

        public string OwnerId { get; private set; }
    }
}