using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordScoreEventHandler : AbstractHandle<WordExpiredEvent>
    {
        public override void Handle(WordExpiredEvent ev)
        {
            SendTask(new WordExpiredTask(ev.WordId, ev.OwnerId), ev);
        }
    }
}