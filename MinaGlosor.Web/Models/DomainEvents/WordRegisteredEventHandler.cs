using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordRegisteredEventHandler : AbstractHandle<WordRegisteredEvent>
    {
        public override void Handle(WordRegisteredEvent ev)
        {
            SendTask(new AddWordToWordListTask(ev.WordListId), ev);
        }
    }
}