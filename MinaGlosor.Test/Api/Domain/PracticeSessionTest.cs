using System;
using System.Linq;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.DomainEvents;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api.Domain
{
    [TestFixture]
    public class PracticeSessionTest
    {
        private PracticeSession practiceSession;
        private DomainEventReset disabler;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            disabler = DomainEvent.Disable();
            var practiceWords = Enumerable.Range(1, 10).Select(i =>
                {
                    SystemTime.UtcDateTime = () => new DateTime(2012, 1, 1).AddDays(i);
                    var word = new Word("words/" + i, "t" + i, "d" + i, "WordLists/1");
                    var practiceWord = new PracticeWord(word, "WordLists/1", "users/1");
                    return practiceWord;
                }).ToArray();
            practiceSession = new PracticeSession("PracticeSession/1", "WordLists/1", practiceWords, "users/1");
        }

        [TearDown]
        public void TearDown()
        {
            disabler.Dispose();
        }

        [Test]
        public void GetsNextWord()
        {
            // Act
            var nextWord = practiceSession.GetNextWord();

            // Assert
            Assert.That(nextWord.WordId, Is.EqualTo("words/1"));
        }

        [Test]
        public void IsUnfinished()
        {
            // Assert
            Assert.That(practiceSession.IsFinished, Is.False);
        }

        [Test]
        public void GetsAllWords()
        {
            // Act
            for (var i = 0; i < 10; i++)
            {
                var nextWord = practiceSession.GetNextWord();
                practiceSession.UpdateConfidence(nextWord.PracticeWordId, ConfidenceLevel.PerfectResponse);
            }

            // Assert
            Assert.That(practiceSession.IsFinished, Is.True);
        }

        [Test]
        public void GetsNewWord()
        {
            // Arrange
            var firstWord = practiceSession.GetNextWord();
            practiceSession.UpdateConfidence(firstWord.PracticeWordId, ConfidenceLevel.CorrectAfterHesitation);

            // Act
            var secondWord = practiceSession.GetNextWord();

            // Assert
            Assert.That(secondWord.PracticeWordId, Is.Not.EqualTo(firstWord.PracticeWordId));
        }

        [Test]
        public void GetsNewWordAfterAllHaveBeenScored()
        {
            // Arrange
            for (var i = 0; i < 10; i++)
            {
                int i1 = i;
                SystemTime.UtcDateTime = () => new DateTime(2013, 1, 1).AddDays(i1);
                var word = practiceSession.GetNextWord();
                practiceSession.UpdateConfidence(word.PracticeWordId, ConfidenceLevel.IncorrectButRemembered);
            }

            SystemTime.UtcDateTime = () => new DateTime(2013, 1, 1).AddDays(11);
            var firstWord = practiceSession.GetNextWord();
            practiceSession.UpdateConfidence(firstWord.PracticeWordId, ConfidenceLevel.CorrectAfterHesitation);

            // Act
            SystemTime.UtcDateTime = () => new DateTime(2013, 1, 1).AddDays(12);
            var secondWord = practiceSession.GetNextWord();

            // Assert
            Assert.That(secondWord.PracticeWordId, Is.Not.EqualTo(firstWord.PracticeWordId));
        }
    }
}