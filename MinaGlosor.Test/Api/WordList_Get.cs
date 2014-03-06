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
        private IdGenerator generator;

        [Test]
        public void GetsEmptyWordList()
        {
            // Arrange
            Transact(session =>
            {
                var owner = new User("e@d.com", "pwd");
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

        protected override void OnSetUp(Castle.Windsor.IWindsorContainer container)
        {
            generator = new IdGenerator();
        }

        private void ArrangeThreeWordLists()
        {
            Transact(context =>
            {
                // first word list
                var owner = new User("e@d.com", "pwd") { Id = generator.NextId() };
                context.Users.Add(owner);
                var wordList1 = new WordList("Some name", owner) { Id = 1 };
                context.WordLists.Add(wordList1);
                wordList1.AddWord("Word1", "Definition1");
                wordList1.AddWord("Word2", "Definition2");

                // second word list
                var wordList2 = new WordList("Then one more", owner) { Id = 2 };
                context.WordLists.Add(wordList2);
                wordList2.AddWord("Word1", "Definition1");
                wordList2.AddWord("Word2", "Definition2");
                wordList2.AddWord("Word3", "Definition2");

                // third one, this is for another user
                var anotherOwner = new User("some_other_user@d.com", "pwd");
                context.Users.Add(anotherOwner);
                var wordList3 = new WordList("Again one more", anotherOwner) { Id = 3 };
                context.WordLists.Add(wordList3);
                wordList3.AddWord("Word1", "Definition1");
            });
        }
    }
}