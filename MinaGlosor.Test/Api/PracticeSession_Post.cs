using System.Globalization;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class PracticeSession_Post : WebApiIntegrationTest
    {
        private PracticeSessionExtensions.PracticeSessionResponse content;

        [Test]
        public void CreatesPracticeSession()
        {
            // Assert
            Assert.That(content.PracticeSessionId, Is.EqualTo("1"));
            PracticeSession practiceSession = null;
            Transact(session =>
            {
                practiceSession = session.Load<PracticeSession>(PracticeSession.ToId(content.PracticeSessionId));
            });
            Assert.That(practiceSession, Is.Not.Null);
            Assert.That(practiceSession.WordListId, Is.EqualTo("WordLists/1"));
        }

        [Test]
        public void AddsWordsToPracticeSession()
        {
            // Assert
            PracticeSession practiceSession = null;
            Transact(session =>
            {
                practiceSession = session.Load<PracticeSession>(PracticeSession.ToId(content.PracticeSessionId));
            });

            Assert.That(practiceSession.Words, Has.Length.EqualTo(10));
        }

        protected override async void Act()
        {
            // Arrange
            Transact(session =>
            {
                var owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                session.Store(owner);
            });

            var wordListResponse = await this.PostWordList("list");
            await this.PublishWordList(wordListResponse.WordListId, true);

            // add some words to the word list
            for (var i = 0; i < 20; i++)
            {
                await this.PostWord(
                    i.ToString(CultureInfo.InvariantCulture),
                    i.ToString(CultureInfo.InvariantCulture),
                    wordListResponse.WordListId);
            }

            // Act
            content = await this.StartPracticeSession("1");
        }
    }
}