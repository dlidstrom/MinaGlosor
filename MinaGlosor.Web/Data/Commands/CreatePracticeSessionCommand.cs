using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Commands
{
    public class CreatePracticeSessionCommand : ICommand
    {
        private readonly int wordListId;

        public CreatePracticeSessionCommand(int wordListId)
        {
            this.wordListId = wordListId;
        }

        public async Task ExecuteAsync(IDbContext context)
        {
            var current = await context.PracticeSessions.Where(x => x.ValidTo == null).ToArrayAsync();
            foreach (var practiceSession in current)
            {
                practiceSession.MarkAsDone();
            }

            var wordList = await context.WordLists.SingleAsync(x => x.Id == wordListId);

            context.PracticeSessions.Add(new PracticeSession(wordList));
        }
    }
}