using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Commands
{
    public class CreatePracticeSessionCommand : ICommand
    {
        private const int NumberOfWordsToPractice = 10;
        private readonly User owner;
        private readonly int wordListId;

        public CreatePracticeSessionCommand(User owner, int wordListId)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            this.owner = owner;
            this.wordListId = wordListId;
        }

        public async Task ExecuteAsync(IDbContext context)
        {
            var current = await context.PracticeSessions.Where(x => x.ValidTo == null)
                                       .ToArrayAsync();
            foreach (var practiceSession in current)
            {
                practiceSession.MarkAsDone();
            }

            var wordScoreWordIds = context.WordScores.Select(x => x.WordId);
            var wordsWithoutScores = await context.Words
                                                  .Where(x => x.WordListId == wordListId
                                                              && !wordScoreWordIds.Contains(x.Id))
                                                  .Take(NumberOfWordsToPractice)
                                                  .ToArrayAsync();
            var practiceWords = wordsWithoutScores.Select(x => new PracticeWord(new WordScore(owner, x)))
                                                  .ToList();
            if (practiceWords.Count < NumberOfWordsToPractice)
            {
                // add words with lowest score
                var query = context.WordScores
                                   .Include(x => x.Word)
                                   .Where(x => x.UserId == owner.Id)
                                   .OrderBy(x => x.EasynessFactor)
                                   .Take(NumberOfWordsToPractice - practiceWords.Count);
                var wordsWithLowestScore = await query.ToArrayAsync();
                var scorePrioritizedPracticeWords = wordsWithLowestScore
                    .Select(x => new PracticeWord(x));
                practiceWords.AddRange(scorePrioritizedPracticeWords);
            }

            var wordList = await context.WordLists.SingleAsync(x => x.Id == wordListId);
            context.PracticeSessions.Add(new PracticeSession(wordList, practiceWords));
        }
    }
}