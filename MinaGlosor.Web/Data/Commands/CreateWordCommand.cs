using System.Data.Entity;
using System.Threading.Tasks;

namespace MinaGlosor.Web.Data.Commands
{
    public class CreateWordCommand : ICommand
    {
        private readonly int wordListId;
        private readonly string text;
        private readonly string definition;

        public CreateWordCommand(int wordListId, string text, string definition)
        {
            this.wordListId = wordListId;
            this.text = text;
            this.definition = definition;
        }

        public async Task ExecuteAsync(IDbContext context)
        {
            var wordList = await context.WordLists.SingleAsync(x => x.Id == wordListId);
            wordList.AddWord(text, definition);
        }
    }
}