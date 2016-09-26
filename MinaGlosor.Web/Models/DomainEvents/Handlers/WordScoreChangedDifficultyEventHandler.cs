using MinaGlosor.Web.Models.BackgroundTasks;

namespace MinaGlosor.Web.Models.DomainEvents.Handlers
{
    public class WordScoreChangedDifficultyEventHandler : AbstractHandle<WordScoreChangedDifficultyEvent>
    {
        public override void Handle(WordScoreChangedDifficultyEvent ev)
        {
            var task = new WordScoreChangedDifficultyTask(
                ev.ModelId,
                ev.WordListId,
                ev.OwnerId,
                ev.WordDifficulty,
                ev.WordScoreDifficultyLifecycle);
            SendTask(task, ev);
        }
    }
}