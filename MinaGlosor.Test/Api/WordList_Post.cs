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
    public class WordList_Post : WebApiIntegrationTest
    {
        [Test]
        public void ItShouldCreateWordList()
        {
            // Arrange
            var owner = new User("First", "Last", "e@d.com", "pwd");
            Transact(session => session.Users.Add(owner));

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("e@d.com"), new string[0]);

            // Act
            var request = new
            {
                wordListName = "Some name"
            };
            var response = Client.PostAsJsonAsync("http://temp.uri/api/wordlist", request).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Transact(session =>
            {
                var wordList = session.WordLists.SingleOrDefault();
                Assert.That(wordList, Is.Not.Null);
                Debug.Assert(wordList != null, "task != null");
                Assert.That(wordList.Name, Is.EqualTo("Some name"));
                Assert.That(wordList.OwnerId, Is.EqualTo(owner.Id));
            });
        }
    }
}