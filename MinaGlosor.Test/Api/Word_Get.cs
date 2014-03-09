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
                    id = 1,
                    text = "w1",
                    definition = "d1"
                },
                new
                {
                    id = 2,
                    text = "w2",
                    definition = "d2"
                }
            };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        [Test]
        public void GetsSingleWord()
        {
            // Act
            var response = Client.GetAsync("http://temp.uri/api/word?id=1").Result;
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = content.ReadAsStringAsync().Result;
            var expected = new
                {
                    id = 1,
                    wordListId = 1,
                    text = "w1",
                    definition = "d1"
                };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        protected override void OnSetUp(Castle.Windsor.IWindsorContainer container)
        {
            // Arrange
            var owner = new User("e@d.com", "pwd") { Id = 1 };
            var wordList = new WordList("list", owner) { Id = 1 };
            Transact(context =>
            {
                context.Users.Add(owner);
                context.WordLists.Add(wordList);
                var word1 = wordList.AddWord("w1", "d1");
                word1.Id = 1;
                context.Words.Add(word1);
                var word2 = wordList.AddWord("w2", "d2");
                word2.Id = 2;
                context.Words.Add(word2);
            });

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("e@d.com"), new string[0]);
        }
    }
}