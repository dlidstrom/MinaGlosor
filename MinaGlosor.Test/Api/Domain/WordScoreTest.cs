using System;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.DomainEvents;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api.Domain
{
    [TestFixture]
    public class WordScoreTest
    {
        private DomainEventReset disabler;

        [SetUp]
        public void SetUp()
        {
            SystemTime.UtcDateTime = () => new DateTime(2012, 1, 1);
            disabler = DomainEvent.Disable();
        }

        [TearDown]
        public void TearDown()
        {
            SystemTime.UtcDateTime = null;
            disabler.Dispose();
        }

        [Test]
        public void RepeatsNextDay()
        {
            // Act
            var wordScore = new WordScore("users/1", "words/1", "WordLists/1");
            wordScore.ScoreWord(ConfidenceLevel.PerfectResponse);

            // Assert
            Assert.That(wordScore.RepeatAfterDate, Is.EqualTo(new DateTime(2012, 1, 2)));
        }

        [Test]
        public void ScoredWordRepeatsInSixDays()
        {
            // Arrange
            var wordScore = new WordScore("users/1", "words/1", "WordLists/1");
            wordScore.ScoreWord(ConfidenceLevel.PerfectResponse);
            SystemTime.UtcDateTime = () => wordScore.RepeatAfterDate;

            // Act
            wordScore.ScoreWord(ConfidenceLevel.PerfectResponse);

            // Assert
            Assert.That(wordScore.RepeatAfterDate, Is.EqualTo(new DateTime(2012, 1, 8)));
        }

        [Test]
        public void DoubleScoredWordRepeatsInManyDays()
        {
            // Arrange
            var wordScore = new WordScore("users/1", "words/1", "WordLists/1");

            // Act
            wordScore.ScoreWord(ConfidenceLevel.PerfectResponse);
            SystemTime.UtcDateTime = () => wordScore.RepeatAfterDate;
            wordScore.ScoreWord(ConfidenceLevel.PerfectResponse);
            SystemTime.UtcDateTime = () => wordScore.RepeatAfterDate;
            wordScore.ScoreWord(ConfidenceLevel.PerfectResponse);

            // Assert
            Assert.That(wordScore.RepeatAfterDate, Is.EqualTo(new DateTime(2012, 1, 25)));
        }

        [Test]
        public void DoubleScoredForgottenRepeatsSoon()
        {
            // Arrange
            var wordScore = new WordScore("users/1", "words/1", "WordLists/1");

            // Act
            wordScore.ScoreWord(ConfidenceLevel.PerfectResponse);
            SystemTime.UtcDateTime = () => wordScore.RepeatAfterDate;
            wordScore.ScoreWord(ConfidenceLevel.PerfectResponse);
            SystemTime.UtcDateTime = () => wordScore.RepeatAfterDate;
            wordScore.ScoreWord(ConfidenceLevel.CompleteBlackout);

            // Assert
            Assert.That(wordScore.RepeatAfterDate, Is.EqualTo(new DateTime(2012, 1, 9)));
        }
    }
}