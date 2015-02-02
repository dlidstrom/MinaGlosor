using System;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.DomainEvents;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api.Domain
{
    [TestFixture]
    public class WordScore_WhenPracticingEarly
    {
        private DomainEventReset disabler;
        private WordScore wordScore;

        [SetUp]
        public void SetUp()
        {
            SystemTime.UtcDateTime = () => new DateTime(2012, 1, 1);
            disabler = DomainEvent.Disable();
            wordScore = new WordScore("users/1", "words/1", "WordLists/1");
            wordScore.ScoreWord(ConfidenceLevel.PerfectResponse);
        }

        [TearDown]
        public void TearDown()
        {
            SystemTime.UtcDateTime = null;
            disabler.Dispose();
        }

        [Test]
        public void DoesNotChangeRepeatAfterDateWhenPerfectResponse()
        {
            // Arrange
            var repeatAfterDate = wordScore.RepeatAfterDate;

            // Act
            wordScore.ScoreWord(ConfidenceLevel.PerfectResponse);

            // Assert
            Assert.That(wordScore.RepeatAfterDate, Is.EqualTo(repeatAfterDate));
        }

        [Test]
        public void DoesNotChangeRepeatAfterDateWhenCorrectAfterHesitation()
        {
            // Arrange
            var repeatAfterDate = wordScore.RepeatAfterDate;

            // Act
            wordScore.ScoreWord(ConfidenceLevel.CorrectAfterHesitation);

            // Assert
            Assert.That(wordScore.RepeatAfterDate, Is.EqualTo(repeatAfterDate));
        }

        [Test]
        public void DoesNotChangeScoreWhenPerfectResponse()
        {
            // Arrange
            var score = wordScore.Score;

            // Act
            wordScore.ScoreWord(ConfidenceLevel.PerfectResponse);

            // Assert
            Assert.That(wordScore.Score, Is.EqualTo(score));
        }

        [Test]
        public void DoesNotChangeScoreWhenCorrectAfterHesitation()
        {
            // Arrange
            var score = wordScore.Score;

            // Act
            wordScore.ScoreWord(ConfidenceLevel.CorrectAfterHesitation);

            // Assert
            Assert.That(wordScore.Score, Is.EqualTo(score));
        }

        [Test]
        public void ChangesRepeatAfterDateWhenCorrectAfterRecalledWithSeriousDifficulty()
        {
            // Arrange
            var repeatAfterDate = wordScore.RepeatAfterDate;

            // Act
            wordScore.ScoreWord(ConfidenceLevel.RecalledWithSeriousDifficulty);

            // Assert
            Assert.That(wordScore.RepeatAfterDate, Is.EqualTo(repeatAfterDate));
        }

        [Test]
        public void ChangesScoreWhenRecalledWithSeriousDifficulty()
        {
            // Arrange
            var score = wordScore.Score;

            // Act
            wordScore.ScoreWord(ConfidenceLevel.RecalledWithSeriousDifficulty);

            // Assert
            Assert.That(wordScore.Score, Is.LessThan(score));
        }
    }
}