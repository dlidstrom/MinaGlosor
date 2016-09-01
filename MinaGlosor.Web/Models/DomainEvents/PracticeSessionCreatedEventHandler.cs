using MinaGlosor.Web.Models.BackgroundTasks;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class PracticeSessionCreatedEventHandler : AbstractHandle<PracticeSessionCreatedEvent>
    {
        public override void Handle(PracticeSessionCreatedEvent ev)
        {
            var progressId = Progress.GetIdFromWordListForUser(ev.WordListId, ev.OwnerId);
            var progress = ExecuteQuery(new GetProgressQuery(progressId));
            if (progress == null)
            {
                SendTask(new CreateWordListProgressTask(ev.WordListId, ev.OwnerId), ev);
            }
        }
    }
}