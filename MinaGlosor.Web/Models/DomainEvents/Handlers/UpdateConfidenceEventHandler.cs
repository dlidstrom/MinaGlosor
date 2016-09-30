using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public class UpdateConfidenceEventHandler : AbstractHandle<UpdateConfidenceLevelEvent>
    {
        public override void Handle(UpdateConfidenceLevelEvent ev)
        {
            SendTask(new ScoreWordTask(ev.WordId, ev.WordListId, ev.ConfidenceLevel, ev.OwnerId), ev);
        }
    }
}