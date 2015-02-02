using System;
using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models.Indexes;
using Raven.Abstractions;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Commands
{
    public class CreatePracticeSessionCommand : ICommand<CreatePracticeSessionCommand.Result>
    {
        private const int WordsToTake = 10;
        private static readonly Random Rng = new Random();
        private readonly string wordListId;
        private readonly string currentUserId;

        public CreatePracticeSessionCommand(string wordListId, User currentUser)
        {
            if (wordListId == null) throw new ArgumentNullException("wordListId");
            if (currentUser == null) throw new ArgumentNullException("currentUser");

            this.wordListId = WordList.ToId(wordListId);
            currentUserId = currentUser.Id;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            var wordList = session.Load<WordList>(wordListId);
            var canExecute = wordList.HasAccess(currentUser.Id);
            if (canExecute == false)
            {
                var owner = session.Load<User>(wordList.OwnerId);
                var message = string.Format("Current user={0}, owner={1}", currentUser.Email, owner.Email);
                TracingLogger.Error(
                    EventIds.Error_Permanent_5XXX.Web_CreatePracticeSession_Unauthorized_5001,
                    message);
            }

            return canExecute;
        }

        public Result Execute(IDocumentSession session)
        {
            // select previously practiced words that are up for new practice
            // if less than 10, fill up with new words that have never been practiced
            var utcNow = SystemTime.UtcNow;
            var wordScoreIdsQuery = from wordScore in session.Query<WordScore, WordScoreIndex>()
                                    where wordScore.WordListId == wordListId
                                        && wordScore.OwnerId == currentUserId
                                        && wordScore.RepeatAfterDate < utcNow
                                    orderby wordScore.RepeatAfterDate
                                    select wordScore.WordId;

            var wordIdsForPractice = new HashSet<string>(wordScoreIdsQuery.Take(WordsToTake).ToArray());

            // while less than 10, fill up with new words that have never been practiced
            var alreadyPracticedWordIdsQuery = from wordScore in session.Query<WordScore, WordScoreIndex>()
                                               where wordScore.WordListId == wordListId
                                                     && wordScore.OwnerId == currentUserId
                                               select wordScore.WordId;
            var alreadyPracticedWordIds = new HashSet<string>();
            var currentWordScoreWordId = 0;
            while (true)
            {
                var practicedWordIdsSubset = alreadyPracticedWordIdsQuery.Skip(currentWordScoreWordId)
                                                                         .Take(128)
                                                                         .ToArray();
                if (practicedWordIdsSubset.Length == 0) break;
                foreach (var practicedWordId in practicedWordIdsSubset)
                {
                    alreadyPracticedWordIds.Add(practicedWordId);
                }

                currentWordScoreWordId += practicedWordIdsSubset.Length;
            }

            var newWordIdsQuery = from word in session.Query<Word, WordIndex>()
                                  where word.WordListId == wordListId
                                  orderby word.CreatedDate
                                  select word.Id;

            var current = 0;
            while (wordIdsForPractice.Count < WordsToTake)
            {
                const int WordIdsToPreload = 100;
                var newWordIds = newWordIdsQuery.Skip(current).Take(WordIdsToPreload).ToArray();
                if (newWordIds.Length == 0) break;

                var wordsNotAlreadyPracticed = newWordIds.Where(x => alreadyPracticedWordIds.Contains(x) == false);
                foreach (var wordNotAlreadyPracticed in wordsNotAlreadyPracticed)
                {
                    wordIdsForPractice.Add(wordNotAlreadyPracticed);
                    if (wordIdsForPractice.Count >= WordsToTake) break;
                }

                current += newWordIds.Length;
            }

            var words = session.Load<Word>(wordIdsForPractice).ToDictionary(x => x.Id);
            var practiceWords = wordIdsForPractice.Select(x => new PracticeWord(words[x], wordListId, currentUserId)).ToArray();
            if (practiceWords.Length == 0)
            {
                var idsOfRandomWords = alreadyPracticedWordIds.OrderBy(x => Rng.Next()).Take(WordsToTake);
                var anyWords = session.Load<Word>(idsOfRandomWords);
                practiceWords = anyWords.Select(x => new PracticeWord(x, wordListId, currentUserId)).ToArray();
            }

            var practiceSession = new PracticeSession(wordListId, practiceWords, currentUserId);
            session.Store(practiceSession);
            return new Result(practiceSession.Id);
        }

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