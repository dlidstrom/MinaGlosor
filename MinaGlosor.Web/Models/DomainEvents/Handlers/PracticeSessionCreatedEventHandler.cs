using MinaGlosor.Web.Models.BackgroundTasks;
using MinaGlosor.Web.Models.Domain.ProgressModel;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public class PracticeSessionCreatedEventHandler : AbstractHandle<PracticeSessionCreatedEvent>
    {
        public override void Handle(PracticeSessionCreatedEvent ev)
        {
            var progressId = Progress.GetIdFromWordListForUser(ev.WordListId, ev.OwnerId);
            var progress = ExecuteQuery(new GetProgressQuery(progressId));
            if (progress == null)
            {
                SendTask(new CreateProgressTask(ev.WordListId, ev.OwnerId), ev);
            }
        }
    }
}