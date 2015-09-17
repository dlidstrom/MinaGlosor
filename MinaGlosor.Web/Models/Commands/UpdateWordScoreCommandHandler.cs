using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Commands.Handlers;
using MinaGlosor.Web.Models.Indexes;

namespace MinaGlosor.Web.Models.Commands
{
    public class UpdateWordScoreCommandHandler : CommandHandlerBase<UpdateWordScoreCommand, object>
    {
        public override object Handle(UpdateWordScoreCommand command)
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

        public override bool CanExecute(UpdateWordScoreCommand command, User currentUser)
        {
            return true;
        }
    }
}