using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    // todo organize models and events into folders
    public class WordRegisteredEventHandler : AbstractHandle<WordRegisteredEvent>
    {
        public override void Handle(WordRegisteredEvent ev)
        {
            SendTask(new AddWordToWordListTask(ev.WordListId), ev);
        }
    }
}