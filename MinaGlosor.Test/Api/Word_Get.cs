using System.Security.Principal;
using System.Threading;
using MinaGlosor.Web.Data.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Word_Get : WebApiIntegrationTest
    {
        [Test]
        public void GetsWordsForWordList()
        {
            // Arrange
            var owner = new User("First", "Last", "e@d.com", "pwd");
            var wordList = new WordList("list", owner);
            Transact(context =>
            {
                context.Users.Add(owner);
                context.WordLists.Add(wordList);
                context.Words.Add(wordList.AddWord("w1", "d1"));
                context.Words.Add(wordList.AddWord("w2", "d2"));
            });

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("e@d.com"), new string[0]);

            // Act
            var response = Client.GetAsync("http://temp.uri/api/word?wordListId=1").Result;
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = content.ReadAsStringAsync().Result;
            var expected = new[]
            {
                new
                {
                    text = "w1",
                    definition = "d1",
                    easynessFactor = 0.0
                },
                new
                {
                    text = "w2",
                    definition = "d2",
                    easynessFactor = 0.0
                }
            };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }
    }
}