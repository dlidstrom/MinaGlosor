using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordConfidenceUpdatedHandler : AbstractHandle<WordConfidenceUpdatedEvent>
    {
        public override void Handle(WordConfidenceUpdatedEvent ev)
        {
            ExecuteCommand(new UpdateWordScoreCommand(ev.WordId, ev.WordListId, ev.ConfidenceLevel, ev.OwnerId), ev);
        }
    }
}