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
    public class Word_Put : WebApiIntegrationTest
    {
        [Test]
        public void UpdatesWord()
        {
            // Arrange
            var owner = new User("e@d.com", "pwd") { Id = 1 };
            Transact(context =>
                {
                    context.Users.Add(owner);
                    var wordList = new WordList("list", owner) { Id = 2 };
                    var word = wordList.AddWord("old text", "old def");
                    word.Id = 3;
                    context.Words.Add(word);
                });

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("e@d.com"), new string[0]);

            // Act
            var request = new
                {
                    id = 3,
                    text = "new word",
                    definition = "new def"
                };
            var response = Client.PutAsJsonAsync("http://temp.uri/api/word", request).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Transact(context =>
                {
                    var newWord = context.Words.Single();
                    Assert.That(newWord.Text, Is.EqualTo("new word"));
                    Assert.That(newWord.Definition, Is.EqualTo("new def"));
                });
        }
    }
}