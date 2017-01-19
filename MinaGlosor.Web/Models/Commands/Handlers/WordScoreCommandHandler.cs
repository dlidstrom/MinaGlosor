using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class WordScoreCommandHandler :
        ICommandHandler<ResetWordScoreCommand, object>,
        ICommandHandler<CheckIfWordExpiresCommand, object>,
        ICommandHandler<ScoreWordCommand, object>
    {
        public IDocumentSession Session { get; set; }

        public object Handle(ResetWordScoreCommand command)
        {
            var wordScore = Session.Load<WordScore>(command.WordScoreId);
            wordScore.ResetAfterWordEdit();
            return null;
        }

        public bool CanExecute(ResetWordScoreCommand command, User currentUser)
        {
            return true;
        }

        public object Handle(CheckIfWordExpiresCommand command)
        {
            var wordScore = Session.Load<WordScore>(command.WordScoreId);
            wordScore.CheckIfWordExpires();
            return new object();
        }

        public bool CanExecute(CheckIfWordExpiresCommand command, User currentUser)
        {
            return true;
        }

        public object Handle(ScoreWordCommand command)
        {
            var wordScore = Session.Query<WordScore, WordScoreIndex>()
                                   .SingleOrDefault(x => x.OwnerId == command.OwnerId && x.WordId == command.WordId);
            if (wordScore == null)
            {
                var id = KeyGeneratorBase.Generate<WordScore>(Session);
                wordScore = new WordScore(id, command.OwnerId, command.WordId, command.WordListId);
                Session.Store(wordScore);
            }

            wordScore.ScoreWord(command.ConfidenceLevel);

            return null;
        }

        public bool CanExecute(ScoreWordCommand command, User currentUser)
        {
            return true;
        }
    }
}