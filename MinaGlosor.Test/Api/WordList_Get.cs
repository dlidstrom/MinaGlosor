using System.Security.Principal;
using System.Threading;
using MinaGlosor.Web.Data.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordList_Get : WebApiIntegrationTest
    {
        [Test]
        public void GetsEmptyWordList()
        {
            // Arrange
            Transact(session =>
            {
                var owner = new User("First", "Last", "e@d.com", "pwd");
                session.Store(owner);
                session.Store(new WordList("list", owner));
            });

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("e@d.com"), new string[0]);

            // Act
            var response = Client.GetAsync("http://temp.uri/api/wordlist").Result;
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = content.ReadAsStringAsync().Result;
            var expected = new[]
            {
                new
                {
                    id = 1,
                    name = "list",
                    wordCount = 0
                }
            };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        [Test]
        public void GetsAllWordListsWithCount()
        {
            // Arrange
            ArrangeThreeWordLists();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("e@d.com"), new string[0]);

            // Act
            var response = Client.GetAsync("http://temp.uri/api/wordlist").Result;
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = content.ReadAsStringAsync().Result;
            var expected = new[]
            {
                new
                {
                    id = 1,
                    name = "Some name",
                    wordCount = 2
                },
                new
                {
                    id = 2,
                    name = "Then one more",
                    wordCount = 3
                }
            };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        [Test]
        public void GetsById()
        {
            // Arrange
            ArrangeThreeWordLists();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("e@d.com"), new string[0]);

            // Act
            var response = Client.GetAsync("http://temp.uri/api/wordlist?id=1").Result;
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = content.ReadAsStringAsync().Result;
            var expected = new
            {
                id = 1,
                name = "Some name",
                wordCount = 2
            };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        private void ArrangeThreeWordLists()
        {
            Transact(session =>
            {
                // first word list
                var owner = new User("First", "Last", "e@d.com", "pwd");
                session.Store(owner);
                var wordList1 = new WordList("Some name", owner);
                session.Store(wordList1);
                session.Store(wordList1.AddWord("Word1", "Definition1"));
                session.Store(wordList1.AddWord("Word2", "Definition2"));

                // second word list
                var wordList2 = new WordList("Then one more", owner);
                session.Store(wordList2);
                session.Store(wordList2.AddWord("Word1", "Definition1"));
                session.Store(wordList2.AddWord("Word2", "Definition2"));
                session.Store(wordList2.AddWord("Word3", "Definition2"));

                // third one, this is for another user
                var anotherOwner = new User("First", "Last", "some_other_user@d.com", "pwd");
                session.Store(anotherOwner);
                var wordList3 = new WordList("Again one more", anotherOwner);
                session.Store(wordList3);
                session.Store(wordList3.AddWord("Word1", "Definition1"));
            });
        }
    }
}