using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.WordListProgressModel;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreateWordListProgressCommand : ICommand<CreateWordListProgressCommand.Result>
    {
        public CreateWordListProgressCommand(string wordListId, string ownerId)
        {
            WordListId = wordListId;
            OwnerId = ownerId;
        }

        public string WordListId { get; private set; }

        public string OwnerId { get; private set; }

        public class Result
        {
            public Result(WordListProgress.Model wordListProgress)
            {
                WordListProgressId = wordListProgress.Id;
            }

            public string WordListProgressId { get; private set; }
        }
    }
}