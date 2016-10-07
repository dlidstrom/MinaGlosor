using MinaGlosor.Web.Models.BackgroundTasks;
using MinaGlosor.Web.Models.Domain.ProgressModel.Queries;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public class AddWordEventHandler : AbstractHandle<AddWordEvent>
    {
        public override void Handle(AddWordEvent ev)
        {
            var result = ExecuteQuery(new GetProgressListByWordListIdQuery(ev.ModelId));
            foreach (var progressId in result.ProgressIds)
            {
                SendTask(new AddWordTask(progressId, ev.NumberOfWords), ev);
            }
        }
    }
}