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
                session.Users.Add(owner);
                session.WordLists.Add(new WordList("list", owner) { Id = 1 });
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
                session.Users.Add(owner);
                var wordList1 = new WordList("Some name", owner);
                session.WordLists.Add(wordList1);
                session.Words.Add(wordList1.AddWord("Word1", "Definition1"));
                session.Words.Add(wordList1.AddWord("Word2", "Definition2"));

                // second word list
                var wordList2 = new WordList("Then one more", owner);
                session.WordLists.Add(wordList2);
                session.Words.Add(wordList2.AddWord("Word1", "Definition1"));
                session.Words.Add(wordList2.AddWord("Word2", "Definition2"));
                session.Words.Add(wordList2.AddWord("Word3", "Definition2"));

                // third one, this is for another user
                var anotherOwner = new User("First", "Last", "some_other_user@d.com", "pwd");
                session.Users.Add(anotherOwner);
                var wordList3 = new WordList("Again one more", anotherOwner);
                session.WordLists.Add(wordList3);
                session.Words.Add(wordList3.AddWord("Word1", "Definition1"));
            });
        }
    }
}