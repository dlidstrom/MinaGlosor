using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public class UpdateWordScoreEventHandler : AbstractHandle<UpdateWordScoreEvent>
    {
        public override void Handle(UpdateWordScoreEvent ev)
        {
            SendTask(new UpdateWordScoreTask(ev.ModelId, ev.WordId, ev.OwnerId, ev.WordScoreDifficultyLifecycle), ev);
        }
    }
}