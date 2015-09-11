using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreatePracticeSessionCommand : ICommand<CreatePracticeSessionCommand.Result>
    {
        public CreatePracticeSessionCommand(string wordListId, User currentUser)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (currentUser == null) throw new ArgumentNullException("currentUser");

            WordListId = WordList.ToId(wordListId);
            CurrentUserId = currentUser.Id;
        }

        public string WordListId { get; private set; }

        public string CurrentUserId { get; private set; }

        public class Result
        {
            public Result(string practiceSessionId)
            {
                if (practiceSessionId == null) throw new ArgumentNullException("practiceSessionId");
                PracticeSessionId = PracticeSession.FromId(practiceSessionId);
            }

            public string PracticeSessionId { get; private set; }
        }
    }
}