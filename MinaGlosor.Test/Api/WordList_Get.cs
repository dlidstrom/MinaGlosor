using MinaGlosor.Web.Models;

using Newtonsoft.Json;

using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordList_Get : WebApiIntegrationTest
    {
        [Test]
        public async void GetsEmptyWordList()
        {
            // Arrange
            Transact(session =>
            {
                var owner = new User("e@d.com", "pwd", "username");
                session.Store(owner);
                session.Store(new WordList("list", owner));
            });

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/wordlist");
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = await content.ReadAsStringAsync();
            var expected = new[]
            {
                new
                {
                    wordListId = "1",
                    ownerId = "1",
                    name = "list",
                    numberOfWords = 0,
                    percentDone = 0
                }
            };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        [Test]
        public async void GetsAllWordListsWithCount()
        {
            // Arrange
            ArrangeThreeWordLists();

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/wordlist");
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = await content.ReadAsStringAsync();
            var expected = new[]
            {
                new
                {
                    wordListId = "1",
                    ownerId = "1",
                    name = "Some name",
                    numberOfWords = 2,
                    percentDone = 0
                },
                new
                {
                    wordListId = "2",
                    ownerId = "1",
                    name = "Then one more",
                    numberOfWords = 3,
                    percentDone = 0
                }
            };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        [Test]
        public async void GetsById()
        {
            // Arrange
            ArrangeThreeWordLists();

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/wordlist?wordListId=1");
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = await content.ReadAsStringAsync();
            var expected = new
            {
                wordListId = "1",
                name = "Some name",
                numberOfWords = 2
            };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        private void ArrangeThreeWordLists()
        {
            Transact(session =>
                {
                    // first word list
                    var owner = new User("e@d.com", "pwd", "username");
                    session.Store(owner);
                    var wordList1 = new WordList("Some name", owner);
                    session.Store(wordList1);

                    session.Store(new Word("Word1", "Def1", wordList1.Id));
                    session.Store(new Word("Word1", "Def1", wordList1.Id));

                    // second word list
                    var wordList2 = new WordList("Then one more", owner);
                    session.Store(wordList2);
                    session.Store(new Word("Word1", "Definition1", wordList2.Id));
                    session.Store(new Word("Word2", "Definition2", wordList2.Id));
                    session.Store(new Word("Word3", "Definition2", wordList2.Id));
                });
        }
    }
}