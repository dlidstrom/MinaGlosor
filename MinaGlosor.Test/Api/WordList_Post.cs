using System.Diagnostics;
using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Models;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordList_Post : WebApiIntegrationTest
    {
        [Test]
        public async void ItShouldCreateWordList()
        {
            // Arrange
            Transact(session =>
                {
                    var user = new User("e@d.com", "pwd", "username");
                    session.Store(user);
                });

            // Act
            var request = new
            {
                name = "Some name"
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/wordlist", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Transact(session =>
                {
                    var wordList = session.Load<WordList>("WordLists/1");
                    Assert.That(wordList, Is.Not.Null);
                    Debug.Assert(wordList != null, "task != null");
                    Assert.That(wordList.Name, Is.EqualTo("Some name"));
                    Assert.That(wordList.OwnerId, Is.EqualTo("users/1"));
                });
        }
    }
}