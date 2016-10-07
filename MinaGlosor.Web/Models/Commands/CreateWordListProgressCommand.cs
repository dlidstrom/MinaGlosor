using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Domain.ProgressModel;

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
            public Result(Progress progress)
            {
                ProgressId = progress.Id;
            }

            public string ProgressId { get; private set; }
        }
    }
}