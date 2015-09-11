using System.Net;
using System.Net.Http;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Word_Get : WebApiIntegrationTest
    {
        [Test]
        public async void GetsSingleWord()
        {
            // Arrange
            Transact(session =>
            {
                var owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                session.Store(owner);
                var wordList = new WordList(KeyGeneratorBase.Generate<WordList>(session), "list", owner.Id);
                session.Store(wordList);
                var generator = new KeyGenerator<Word>(session);
                var word = Word.Create(
                    generator.Generate(),
                    "some text",
                    "some def",
                    wordList);
                session.Store(word);
            });

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/word?wordId=1");
            var content = await response.Content.ReadAsAsync<ExpectedContent>();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content, Is.Not.Null);
            Assert.That(content.WordId, Is.EqualTo("1"));
            Assert.That(content.WordListId, Is.EqualTo("1"));
            Assert.That(content.Text, Is.EqualTo("some text"));
            Assert.That(content.Definition, Is.EqualTo("some def"));
        }

        public class ExpectedContent
        {
            public string WordId { get; set; }

            public string WordListId { get; set; }

            public string Text { get; set; }

            public string Definition { get; set; }
        }
    }
}