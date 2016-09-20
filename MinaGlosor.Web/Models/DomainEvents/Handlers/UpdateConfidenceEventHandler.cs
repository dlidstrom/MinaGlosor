using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public class UpdateConfidenceEventHandler : AbstractHandle<UpdateConfidenceEvent>
    {
        public override void Handle(UpdateConfidenceEvent ev)
        {
            SendTask(new UpdateWordScoreTask(ev.WordId, ev.WordListId, ev.ConfidenceLevel, ev.OwnerId), ev);
        }
    }
}