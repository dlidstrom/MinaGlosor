using System;
using System.Collections.Generic;
using System.Linq;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models.Indexes;
using Raven.Abstractions;
using Raven.Client;
using Raven.Client.Linq;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class PracticeSessionCommandHandler :
        ICommandHandler<CreatePracticeSessionCommand, CreatePracticeSessionCommand.Result>,
        ICommandHandler<UpdateLastPickedDateCommand, object>,
        ICommandHandler<UpdateWordConfidenceCommand, UpdateWordConfidenceCommand.Result>
    {
        private const int WordsToTake = 10;
        private static readonly Random Rng = new Random();

        public IDocumentSession Session { get; set; }

        public CreatePracticeSessionCommand.Result Handle(CreatePracticeSessionCommand command)
        {
            var utcNow = SystemTime.UtcNow;
            
            var wordIdsForPractice = new List<string>();
            FillWithNewWords(Session, wordIdsForPractice, command.WordListId, command.CurrentUserId);
            if (wordIdsForPractice.Count == 0)
            {
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
                wordIdsForPractice.AddRange(difficultWordIdsForPractice.Concat(easyWordIdsForPractice));

                // while less than 10, fill up with new words that have never been practiced
                //if (wordIdsForPractice.Count < WordsToTake)
                //{
                //    FillWithNewWords(Session, wordIdsForPractice, command.WordListId, command.CurrentUserId);
                //}
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

        public bool CanExecute(CreatePracticeSessionCommand command, User currentUser)
        {
            var wordList = Session.Load<WordList>(command.WordListId);
            var isPublished = wordList.IsPublished();
            if (isPublished == false)
            {
                var owner = Session.Load<User>(wordList.OwnerId);
                var message = string.Format("Current user={0}, owner={1}", currentUser.Email, owner.Email);
                TracingLogger.Error(
                    EventIds.Error_Permanent_5XXX.Web_CreatePracticeSession_Unauthorized_5001,
                    message);
            }

            return isPublished;
        }

        public object Handle(UpdateLastPickedDateCommand command)
        {
            var practiceSession = Session.Load<PracticeSession>(command.PracticeSessionId);
            practiceSession.UpdateLastPickedDate(command.PracticeWordId);
            return null;
        }

        public bool CanExecute(UpdateLastPickedDateCommand command, User currentUser)
        {
            return true;
        }

        public UpdateWordConfidenceCommand.Result Handle(UpdateWordConfidenceCommand command)
        {
            var practiceSession = Session.Load<PracticeSession>(command.PracticeSessionId);
            practiceSession.UpdateConfidence(command.PracticeWordId, command.ConfidenceLevel);
            return new UpdateWordConfidenceCommand.Result(practiceSession.GetStatistics());
        }

        public bool CanExecute(UpdateWordConfidenceCommand command, User currentUser)
        {
            var practiceSession = Session.Load<PracticeSession>(command.PracticeSessionId);
            var hasAccess = practiceSession.HasAccess(currentUser.Id);
            return hasAccess;
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