using System;
using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.Indexes;
using Raven.Abstractions;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class CreatePracticeSessionCommandHandler : CommandHandlerBase<CreatePracticeSessionCommand, CreatePracticeSessionCommand.Result>
    {
        private const int WordsToTake = 10;
        private static readonly Random Rng = new Random();

        public override CreatePracticeSessionCommand.Result Handle(CreatePracticeSessionCommand command)
        {
            var utcNow = SystemTime.UtcNow;

            // select difficult words that are up for new practice
            var difficultWordScoreIdsQuery = from wordScore in Session.Query<WordScore, WordScoreIndex>()
                                             where wordScore.WordListId == command.WordListId
                                                   && wordScore.OwnerId == command.CurrentUserId
                                                   && wordScore.RepeatAfterDate < utcNow
                                                   && wordScore.WordDifficulty == WordDifficulty.Difficult
                                             orderby wordScore.RepeatAfterDate
                                             select wordScore.WordId;

            var easyWordScoreIdsQuery = from wordScore in Session.Query<WordScore, WordScoreIndex>()
                                        where wordScore.WordListId == command.WordListId
                                              && wordScore.OwnerId == command.CurrentUserId
                                              && wordScore.RepeatAfterDate < utcNow
                                              && wordScore.WordDifficulty == WordDifficulty.Easy
                                        orderby wordScore.RepeatAfterDate
                                        select wordScore.WordId;

            var difficultWordIdsForPractice = difficultWordScoreIdsQuery.Take(WordsToTake).ToArray();
            var easyWordIdsForPractice = easyWordScoreIdsQuery.Take(WordsToTake - difficultWordIdsForPractice.Length).ToList();
            var wordIdsForPractice = difficultWordIdsForPractice.Concat(easyWordIdsForPractice).ToList();

            // while less than 10, fill up with new words that have never been practiced
            if (wordIdsForPractice.Count < WordsToTake)
            {
                FillWithNewWords(Session, wordIdsForPractice, command.WordListId, command.CurrentUserId);
            }

            var words = Session.Load<Word>(wordIdsForPractice).ToDictionary(x => x.Id);
            var practiceWords = wordIdsForPractice.Select(x => new PracticeWord(words[x], command.WordListId, command.CurrentUserId)).ToArray();
            if (practiceWords.Length == 0)
            {
                practiceWords = GetRandomPracticeWords(Session, command.WordListId, command.CurrentUserId);
            }

            var practiceSession = new PracticeSession(
                KeyGeneratorBase.Generate<PracticeSession>(Session),
                command.WordListId,
                practiceWords,
                command.CurrentUserId);
            Session.Store(practiceSession);
            return new CreatePracticeSessionCommand.Result(practiceSession.Id);
        }

        public override bool CanExecute(CreatePracticeSessionCommand command, User currentUser)
        {
            // TODO: Check if word list is published?
            return true;
            //var wordList = Session.Load<WordList>(command.WordListId);
            //var canExecute = wordList.HasAccess(currentUser.Id);
            //if (canExecute == false)
            //{
            //    var owner = Session.Load<User>(wordList.OwnerId);
            //    var message = string.Format("Current user={0}, owner={1}", currentUser.Email, owner.Email);
            //    TracingLogger.Error(
            //        EventIds.Error_Permanent_5XXX.Web_CreatePracticeSession_Unauthorized_5001,
            //        message);
            //}

            //return canExecute;
        }

        private void FillWithNewWords(IDocumentSession session, List<string> wordIdsForPractice, string wordListId, string currentUserId)
        {
            var alreadyPracticedWordIds = GetAlreadyPracticedWordIds(session, wordListId, currentUserId);
            var current = 0;
            var newWordIdsQuery = from word in session.Query<Word, WordIndex>()
                                  where word.WordListId == wordListId
                                  orderby word.CreatedDate
                                  select word.Id;
            while (wordIdsForPractice.Count < WordsToTake)
            {
                const int WordIdsToPreload = 128;
                var newWordIds = newWordIdsQuery.Skip(current)
                                                .Take(WordIdsToPreload)
                                                .ToArray();
                if (newWordIds.Length == 0) break;

                var wordsNotAlreadyPracticed = newWordIds.Where(x => alreadyPracticedWordIds.Contains(x) == false);
                foreach (var wordNotAlreadyPracticed in wordsNotAlreadyPracticed)
                {
                    wordIdsForPractice.Add(wordNotAlreadyPracticed);
                    if (wordIdsForPractice.Count >= WordsToTake) break;
                }

                current += newWordIds.Length;
            }
        }

        private PracticeWord[] GetRandomPracticeWords(IDocumentSession session, string wordListId, string currentUserId)
        {
            var alreadyPracticedWordIds = GetAlreadyPracticedWordIds(session, wordListId, currentUserId);
            var idsOfRandomWords = alreadyPracticedWordIds.OrderBy(x => Rng.Next()).Take(WordsToTake);
            var anyWords = session.Load<Word>(idsOfRandomWords);
            var practiceWords = anyWords.Select(x => new PracticeWord(x, wordListId, currentUserId)).ToArray();
            return practiceWords;
        }

        private HashSet<string> GetAlreadyPracticedWordIds(IDocumentSession session, string wordListId, string currentUserId)
        {
            var alreadyPracticedWordIdsQuery = from wordScore in session.Query<WordScore, WordScoreIndex>()
                                               where wordScore.WordListId == wordListId
                                                     && wordScore.OwnerId == currentUserId
                                               select wordScore.WordId;
            var alreadyPracticedWordIds = new HashSet<string>();
            var currentWordScoreWordId = 0;
            while (true)
            {
                const int WordIdsToPreload = 128;
                var practicedWordIdsSubset = alreadyPracticedWordIdsQuery.Skip(currentWordScoreWordId)
                                                                         .Take(WordIdsToPreload)
                                                                         .ToArray();
                if (practicedWordIdsSubset.Length == 0) break;
                foreach (var practicedWordId in practicedWordIdsSubset)
                {
                    alreadyPracticedWordIds.Add(practicedWordId);
                }

                currentWordScoreWordId += practicedWordIdsSubset.Length;
            }

            return alreadyPracticedWordIds;
        }
    }
}