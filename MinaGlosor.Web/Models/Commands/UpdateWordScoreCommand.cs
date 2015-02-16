using System;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class UpdateWordScoreCommand : ICommand
    {
        private readonly string wordId;
        private readonly ConfidenceLevel confidenceLevel;
        private readonly string ownerId;
        private readonly string wordListId;

        public UpdateWordScoreCommand(string wordId, string wordListId, ConfidenceLevel confidenceLevel, string ownerId)
        {
            if (wordId == null) throw new ArgumentNullException("wordId");
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");

            this.wordId = wordId;
            this.wordListId = wordListId;
            this.confidenceLevel = confidenceLevel;
            this.ownerId = ownerId;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public void Execute(IDocumentSession session)
        {
            var wordScore = session.Query<WordScore, WordScoreIndex>()
                                   .SingleOrDefault(x => x.OwnerId == ownerId && x.WordId == wordId);
            if (wordScore == null)
            {
                wordScore = new WordScore(ownerId, wordId, wordListId);
                session.Store(wordScore);
            }

            wordScore.ScoreWord(confidenceLevel);
        }
    }
}