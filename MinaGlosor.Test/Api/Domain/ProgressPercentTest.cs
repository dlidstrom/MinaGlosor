using MinaGlosor.Web.Models.Domain.ProgressModel;
using NUnit.Framework;

namespace MinaGlosor.Test.Api.Domain
{
    [TestFixture]
    public class ProgressPercentTest
    {
        // will probably need to revisit
        [Test]
        public void RoundsCorrectly()
        {
            // Arrange
            var wordCounts = new ProgressWordCounts();
            wordCounts = wordCounts.IncreaseCount();
            wordCounts = wordCounts.IncreaseCount();
            wordCounts = wordCounts.IncreaseCount();
            wordCounts = wordCounts.IncreaseExpired();
            wordCounts = wordCounts.IncreaseExpired();
            var percents = new ProgressPercentages();

            // Act
            percents = percents.Of(wordCounts, 3);

            // Assert
            Assert.That(percents.PercentExpired, Is.EqualTo(66));
            Assert.That(percents.PercentDone, Is.EqualTo(100));
        }
    }
}