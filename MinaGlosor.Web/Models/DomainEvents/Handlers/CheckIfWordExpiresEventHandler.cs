using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public class CheckIfWordExpiresEventHandler : AbstractHandle<CheckIfWordExpiresEvent>
    {
        public override void Handle(CheckIfWordExpiresEvent ev)
        {
            SendTask(new CheckIfWordExpiresTask(ev.ModelId, ev.WordDifficultyUpdate), ev, ev.RepeatAfterDate);
        }
    }
}