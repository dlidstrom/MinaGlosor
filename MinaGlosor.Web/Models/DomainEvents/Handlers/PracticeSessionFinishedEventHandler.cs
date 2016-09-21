using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public class PracticeSessionFinishedEventHandler : AbstractHandle<PracticeSessionFinishedEvent>
    {
        public override void Handle(PracticeSessionFinishedEvent ev)
        {
            foreach (var wordResult in ev.WordResults)
            {
                SendTask(new UpdateWordScoreDifficultyTask(wordResult.ConfidenceLevels, wordResult.WordId, wordResult.OwnerId), ev);
            }
        }
    }
}