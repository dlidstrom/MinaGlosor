using System;
using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Models;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Word_Put : WebApiIntegrationTest
    {
        [Test]
        public void UpdatesWord()
        {
            // Arrange
            Transact(session =>
                {
                    var owner = new User("e@d.com", "pwd", "username");
                    session.Store(owner);
                    var wordList = new WordList("list", owner);
                    session.Store(wordList);
                    var word = new Word("Words/1", "old text", "old def", wordList.Id, Guid.NewGuid(), null);
                    session.Store(word);
                });

            // Act
            var request = new
                {
                    wordId = "1",
                    text = "new word",
                    definition = "new def"
                };
            var response = Client.PutAsJsonAsync("http://temp.uri/api/word", request).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Transact(session =>
                {
                    var newWord = session.Load<Word>("Words/1");
                    Assert.That(newWord.Text, Is.EqualTo("new word"));
                    Assert.That(newWord.Definition, Is.EqualTo("new def"));
                });
        }
    }
}