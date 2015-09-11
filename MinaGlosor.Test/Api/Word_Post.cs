using System.Diagnostics;
using System.Net;
using System.Net.Http;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Word_Post : WebApiIntegrationTest
    {
        [Test]
        public async void AddsWordToList()
        {
            // Arrange
            Transact(session =>
                {
                    var owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                    session.Store(owner);
                    var wordList = new WordList(KeyGeneratorBase.Generate<WordList>(session), "list", owner.Id);
                    session.Store(wordList);
                });

            // Act
            var request = new
                {
                    wordListId = "1",
                    text = "Some word",
                    definition = "Some definition"
                };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/word", request);
            Assert.That(response.Content, Is.Not.Null);
            var content = await response.Content.ReadAsAsync<ExpectedContent>();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Word savedWord = null;
            Transact(session =>
            {
                savedWord = session.Load<Word>(Word.ToId(content.WordId));
            });
            Assert.That(savedWord, Is.Not.Null);
            Debug.Assert(savedWord != null, "task != null");
            Assert.That(savedWord.Text, Is.EqualTo("Some word"));
            Assert.That(savedWord.Definition, Is.EqualTo("Some definition"));
            Assert.That(savedWord.WordListId, Is.EqualTo("WordLists/1"));
        }

        public class ExpectedContent
        {
            public string WordId { get; set; }
        }
    }
}