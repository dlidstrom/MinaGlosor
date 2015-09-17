using System.Globalization;
using System.Net;
using System.Net.Http;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class PracticeSession_Post : WebApiIntegrationTest
    {
        private HttpResponseMessage response;
        private ExpectedContent content;

        [Test]
        public void CreatesPracticeSession()
        {
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
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

                var wordList = new WordList(KeyGeneratorBase.Generate<WordList>(session), "list", owner.Id);
                session.Store(wordList);

                // add some words to the word list
                var generator = new KeyGenerator<Word>(session);
                for (var i = 0; i < 20; i++)
                {
                    var word = Word.Create(
                        generator.Generate(),
                        i.ToString(CultureInfo.InvariantCulture),
                        i.ToString(CultureInfo.InvariantCulture),
                        wordList);
                    session.Store(word);
                }
            });

            // Act
            var request = new
            {
                wordListId = "1"
            };
            response = await Client.PostAsJsonAsync("http://temp.uri/api/practicesession", request);
            Assert.That(response.Content, Is.Not.Null);
            content = await response.Content.ReadAsAsync<ExpectedContent>();
        }

        public class ExpectedContent
        {
            public string PracticeSessionId { get; set; }
        }
    }
}