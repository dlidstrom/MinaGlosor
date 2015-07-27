using MinaGlosor.Web.Models.BackgroundTasks;
using MinaGlosor.Web.Models.Queries;

namespace MinaGlosor.Web.Models.DomainEvents
{
    public class WordUpdatedEventHandler : AbstractHandle<WordUpdatedEvent>
    {
        public override void Handle(WordUpdatedEvent ev)
        {
            var result = ExecuteQuery(new GetWordScoreIdsQuery(ev.ModelId));
            foreach (var wordScoreId in result.Ids)
            {
                SendTask(new ResetWordScoreEvent(wordScoreId), ev);
            }
        }
    }
}