using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordIsUpToDateEventHandler : AbstractHandle<WordIsUpToDateEvent>
    {
        public override void Handle(WordIsUpToDateEvent ev)
        {
            SendTask(new WordIsUpToDateTask(ev.WordId, ev.OwnerId), ev);
        }
    }
}