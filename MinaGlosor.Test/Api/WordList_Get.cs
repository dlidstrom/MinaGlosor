using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
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
            User owner = null;
            Transact(session =>
            {
                owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                session.Store(owner);
                session.Store(new WordList(KeyGeneratorBase.Generate<WordList>(session), "list", owner.Id));
            });

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/wordlist");
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = await content.ReadAsStringAsync();
            var expected = new
                {
                    wordLists = new[]
                        {
                            new
                                {
                                    wordListId = "1",
                                    ownerId = User.FromId(owner.Id),
                                    name = "list",
                                    numberOfWords = 0,
                                    percentDone = 0,
                                    percentExpired = 0
                                }
                        },
                    numberOfFavourites = 0
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
            var expected = new
                {
                    wordLists = new[]
                        {
                            new
                                {
                                    wordListId = "1",
                                    ownerId = "1",
                                    name = "Some name",
                                    numberOfWords = 2,
                                    percentDone = 0,
                                    percentExpired = 0
                                },
                            new
                                {
                                    wordListId = "2",
                                    ownerId = "1",
                                    name = "Then one more",
                                    numberOfWords = 3,
                                    percentDone = 0,
                                    percentExpired = 0
                                }
                        },
                    numberOfFavourites = 0
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
                    var owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                    session.Store(owner);
                    var wordListGenerator = new KeyGenerator<WordList>(session);
                    var wordList1 = new WordList(wordListGenerator.Generate(), "Some name", owner.Id);
                    session.Store(wordList1);

                    var generator = new KeyGenerator<Word>(session);
                    session.Store(Word.Create(generator.Generate(), "Word1", "Def1", wordList1));
                    session.Store(Word.Create(generator.Generate(), "Word1", "Def1", wordList1));

                    // second word list
                    var wordList2 = new WordList(wordListGenerator.Generate(), "Then one more", owner.Id);
                    session.Store(wordList2);
                    session.Store(Word.Create(generator.Generate(), "Word1", "Definition1", wordList2));
                    session.Store(Word.Create(generator.Generate(), "Word2", "Definition2", wordList2));
                    session.Store(Word.Create(generator.Generate(), "Word3", "Definition2", wordList2));
                });
        }
    }
}