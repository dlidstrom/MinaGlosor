using System;
using MinaGlosor.Web.Infrastructure;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreatePracticeSessionCommand : ICommand<CreatePracticeSessionCommand.Result>
    {
        public CreatePracticeSessionCommand(string wordListId, User currentUser, PracticeMode practiceMode)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (currentUser == null) throw new ArgumentNullException("currentUser");

            WordListId = WordList.ToId(wordListId);
            CurrentUserId = currentUser.Id;
            PracticeMode = practiceMode;
        }

        public string WordListId { get; private set; }

        public string CurrentUserId { get; private set; }

        public PracticeMode PracticeMode { get; private set; }

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