using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class UpdateConfidenceEventHandler : AbstractHandle<UpdateConfidenceEvent>
    {
        public override void Handle(UpdateConfidenceEvent ev)
        {
            ExecuteCommand(new UpdateWordScoreCommand(ev.WordId, ev.WordListId, ev.ConfidenceLevel, ev.OwnerId), ev);
        }
    }
}