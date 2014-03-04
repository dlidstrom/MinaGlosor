using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using MinaGlosor.Web.Data.Models;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Word_Post : WebApiIntegrationTest
    {
        [Test]
        public void AddsWordToList()
        {
            // Arrange
            var owner = new User("First", "Last", "e@d.com", "pwd");
            var wordList = new WordList("list", owner);
            Transact(session =>
            {
                session.Store(owner);
                session.Store(wordList);
            });

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("e@d.com"), new string[0]);

            // Act
            var request = new
            {
                wordListId = 1,
                word = "Some word",
                definition = "Some definition"
            };
            var response = Client.PostAsJsonAsync("http://temp.uri/api/word", request).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Transact(session =>
            {
                var word = session.Load<Word>(1);
                Assert.That(word, Is.Not.Null);
                Debug.Assert(word != null, "task != null");
                Assert.That(word.Text, Is.EqualTo("Some word"));
                Assert.That(word.Definition, Is.EqualTo("Some definition"));
                Assert.That(word.WordListId, Is.EqualTo(wordList.Id));
            });
        }
    }
}