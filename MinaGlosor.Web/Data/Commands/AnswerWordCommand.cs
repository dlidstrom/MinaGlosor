using System;
using MinaGlosor.Web.Data.Models;
using Raven.Client;

namespace MinaGlosor.Web.Data.Commands
{
    public class AnswerWordCommand : ICommand
    {
        private readonly User user;
        private readonly string wordId;
        private readonly string wordListId;
        private readonly int confidence;

        public AnswerWordCommand(User user, string wordId, string wordListId, int confidence)
        {
            if (user == null) throw new ArgumentNullException("user");
            this.user = user;
            this.wordId = wordId;
            this.wordListId = wordListId;
            this.confidence = confidence;
        }

        public void Execute(IDocumentSession session)
        {
            var id = WordAnswer.GetId(wordId, user);
            var wordAnswer = session.Load<WordAnswer>(id);
            if (wordAnswer == null)
            {
                wordAnswer = new WordAnswer(wordId, wordListId, user);
                session.Store(wordAnswer);
            }

            wordAnswer.AddConfidence(confidence);
        }
    }
}