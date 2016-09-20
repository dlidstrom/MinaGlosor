using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public class WordListRegisteredEventHandler : AbstractHandle<WordListRegisteredEvent>
    {
        public override void Handle(WordListRegisteredEvent ev)
        {
            SendTask(new CreateWordListProgressTask(ev.ModelId, ev.OwnerId), ev);
        }
    }
}