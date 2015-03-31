using MinaGlosor.Web.Models.DomainEvents;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordRememberedEventHandlerTest : WebApiIntegrationTest
    {
        [Test]
        public void StoresWordCountForDate()
        {
            // Arrange
            var handler = new WordRememberedEventHandler();

            Transact(session =>
                {
                    // Act
                    handler.Handle(new WordRememberedEvent("users/1"));

                    // Assert
                    var seenWord = session.Load<SeenWords>("seen-words-1-2015-03-30");

                    Assert.That(seenWord, Is.Not.Null);
                    Assert.That(seenWord.NumberOfSeenWords, Is.EqualTo(1));
                });
        }
    }

    public class SeenWords
    {
        public SeenWords()
        {
        }

        public int NumberOfSeenWords { get; private set; }
    }
}