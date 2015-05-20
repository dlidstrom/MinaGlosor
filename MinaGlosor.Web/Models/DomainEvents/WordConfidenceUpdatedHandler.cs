using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordConfidenceUpdatedHandler : AbstractHandle<WordConfidenceUpdated>
    {
        public override void Handle(WordConfidenceUpdated ev)
        {
            ExecuteCommand(new UpdateWordScoreCommand(ev.WordId, ev.WordListId, ev.ConfidenceLevel, ev.OwnerId), ev);
        }
    }
}