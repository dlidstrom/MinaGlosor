using System;
using System.Threading.Tasks;
using MinaGlosor.Web.Data.Models;

namespace MinaGlosor.Web.Data.Commands
{
    public class AnswerWordCommand : ICommand
    {
        private readonly User user;
        private readonly int wordId;
        private readonly int wordListId;
        private readonly int confidence;

        public AnswerWordCommand(User user, int wordId, int wordListId, int confidence)
        {
            if (user == null) throw new ArgumentNullException("user");
            this.user = user;
            this.wordId = wordId;
            this.wordListId = wordListId;
            this.confidence = confidence;
        }

        public Task ExecuteAsync(IDbContext session)
        {
            //var word = await session.Words.SingleOrDefaultAsync(x => x.Id == wordId);
            //var wordList = session.Load<WordList>(wordListId);
            //var id = WordAnswer.GetId(word, user);
            //var wordAnswer = session.Load<WordAnswer>(id);
            //if (wordAnswer == null)
            //{
            //    wordAnswer = wordList.Answer(word, user);
            //    session.Store(wordAnswer);
            //}

            //wordAnswer.UpdateEasynessFactor(confidence);
            return null;
        }
    }
}