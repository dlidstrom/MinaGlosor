using System.Diagnostics;
using System.Linq;
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
            var owner = new User("e@d.com", "pwd") { Id = 1 };
            var wordList = new WordList("list", owner) { Id = 2 };
            Transact(context =>
            {
                context.Users.Add(owner);
                context.WordLists.Add(wordList);
            });

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("e@d.com"), new string[0]);

            // Act
            var request = new
            {
                wordListId = 2,
                text = "Some word",
                definition = "Some definition"
            };
            var response = Client.PostAsJsonAsync("http://temp.uri/api/word", request).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var word = wordList.Words.SingleOrDefault();
            Assert.That(word, Is.Not.Null);
            Debug.Assert(word != null, "task != null");
            Assert.That(word.Text, Is.EqualTo("Some word"));
            Assert.That(word.Definition, Is.EqualTo("Some definition"));
            Assert.That(word.WordListId, Is.EqualTo(wordList.Id));
        }
    }
}