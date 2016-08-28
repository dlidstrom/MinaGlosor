using MinaGlosor.Web.Models.BackgroundTasks;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class PracticeSessionCreatedEventHandler : AbstractHandle<PracticeSessionCreatedEvent>
    {
        public override void Handle(PracticeSessionCreatedEvent ev)
        {
            var wordList = ExecuteQuery(new GetWordListQuery(ev.WordListId));
            if (wordList.OwnerId != User.FromId(ev.OwnerId))
            {
                SendTask(new CreateWordListProgressTask(ev.WordListId, ev.OwnerId), ev);
            }
        }
    }
}