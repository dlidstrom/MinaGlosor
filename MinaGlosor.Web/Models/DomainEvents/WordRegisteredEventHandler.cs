using MinaGlosor.Web.Models.Commands;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordRegisteredEventHandler : AbstractHandle<WordRegisteredEvent>
    {
        public override void Handle(WordRegisteredEvent ev)
        {
            ExecuteCommand(new AddWordToWordListCommand(ev.WordListId));
        }
    }
}