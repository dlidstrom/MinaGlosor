using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public class WordScoreRegisteredEventHandler : AbstractHandle<WordScoreRegisteredEvent>
    {
        public override void Handle(WordScoreRegisteredEvent ev)
        {
            SendTask(new WordScoreRegisteredTask(ev.WordId, ev.OwnerId), ev);
        }
    }
}