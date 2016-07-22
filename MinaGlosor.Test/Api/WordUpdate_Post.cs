using System.Net;
using System.Net.Http;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordUpdate_Post : WebApiIntegrationTest
    {
        [Test]
        public void UpdatesWord()
        {
            // Arrange
            Transact(session =>
                {
                    var owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                    session.Store(owner);
                    var wordList = new WordList(KeyGeneratorBase.Generate<WordList>(session), "list", owner.Id);
                    session.Store(wordList);
                    var generator = new KeyGenerator<Word>(session);
                    var word = Word.Create(generator.Generate(), "old text", "old def", wordList);
                    session.Store(word);
                });

            // Act
            var request = new
                {
                    wordId = "1",
                    text = "new word",
                    definition = "new def"
                };
            var response = Client.PostAsJsonAsync("http://temp.uri/api/updateword", request).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Transact(session =>
                {
                    var newWord = session.Load<Word>("words/1");
                    Assert.That(newWord.Text, Is.EqualTo("new word"));
                    Assert.That(newWord.Definition, Is.EqualTo("new def"));
                });
        }
    }
}