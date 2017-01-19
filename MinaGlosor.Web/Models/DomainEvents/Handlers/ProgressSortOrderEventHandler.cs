using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public class ProgressSortOrderEventHandler : AbstractHandle<AddWordEvent>
    {
        public override void Handle(AddWordEvent ev)
        {
            SendTask(new ProgressNumberOfWordsSortOrderTask(), ev);
        }
    }
}