using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public class WordListRegisteredEventHandler : AbstractHandle<WordListRegisteredEvent>
    {
        public override void Handle(WordListRegisteredEvent ev)
        {
            SendTask(new CreateProgressTask(ev.ModelId, ev.OwnerId), ev);
        }
    }
}