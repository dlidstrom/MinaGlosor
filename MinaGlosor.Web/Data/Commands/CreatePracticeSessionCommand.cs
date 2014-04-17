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
            var toBeExpired = await context.PracticeSessions
                                           .Where(x => x.ValidTo.HasValue == false)
                                           .ToArrayAsync();
            foreach (var expired in toBeExpired)
                expired.MarkAsDone();

            // new practice session
            var wordList = await context.WordLists.SingleAsync(x => x.Id == wordListId);
            var practiceSession = owner.Practice(wordList);
            var wordScoreWordIds = context.WordScores.Select(x => x.WordId);
            var wordsWithoutScores = await context.Words
                                                  .Where(x => x.WordListId == wordListId
                                                              && !wordScoreWordIds.Contains(x.Id))
                                                  .Take(NumberOfWordsToPractice)
                                                  .ToArrayAsync();

            foreach (var wordsWithoutScore in wordsWithoutScores)
            {
                practiceSession.AddPracticeWord(owner.Score(wordsWithoutScore));
            }

            if (wordsWithoutScores.Length < NumberOfWordsToPractice)
            {
                // add words with lowest score
                var query = context.WordScores
                                   .Include(x => x.Word)
                                   .Where(x => x.UserId == owner.Id)
                                   .OrderBy(x => x.EasynessFactor)
                                   .Take(NumberOfWordsToPractice - wordsWithoutScores.Length);
                var wordsWithLowestScore = await query.ToArrayAsync();

                foreach (var wordScore in wordsWithLowestScore)
                {
                    practiceSession.AddPracticeWord(wordScore);
                }
            }

            context.PracticeSessions.Add(practiceSession);
        }
    }
}