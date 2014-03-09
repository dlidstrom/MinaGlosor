using System.Data.Entity;
using System.Threading.Tasks;

namespace MinaGlosor.Web.Data.Commands
{
    public class UpdateWordCommand : ICommand
    {
        private readonly int id;
        private readonly string text;
        private readonly string definition;

        public UpdateWordCommand(int id, string text, string definition)
        {
            this.id = id;
            this.text = text;
            this.definition = definition;
        }

        public async Task ExecuteAsync(IDbContext context)
        {
            var word = await context.Words.SingleAsync(x => x.Id == id);
            word.Update(text, definition);
        }
    }
}