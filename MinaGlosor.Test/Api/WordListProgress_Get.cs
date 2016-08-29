using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordListProgress_Get : WebApiIntegrationTest
    {
        private User owner;

        [Test]
        public async void GetsEmptyWordList()
        {
            // Arrange
            await this.PostWordList("list");

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/wordlistprogress");
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
            var response = await Client.GetAsync("http://temp.uri/api/wordlistprogress");
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
                                    wordListId = "5",
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
                ownerId = "1",
                name = "Some name",
                numberOfWords = 2
            };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        protected override void Arrange()
        {
            Transact(session =>
            {
                owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                session.Store(owner);
            });
        }

        private async void ArrangeThreeWordLists()
        {
            var wordListResult1 = await this.PostWordList("Some name");
            await this.PostWord("Word1", "Def1", wordListResult1.WordListId);
            await this.PostWord("Word2", "Def2", wordListResult1.WordListId);

            var wordListResult2 = await this.PostWordList("Then one more");
            await this.PostWord("Word1", "Def1", wordListResult2.WordListId);
            await this.PostWord("Word2", "Def2", wordListResult2.WordListId);
            await this.PostWord("Word3", "Def3", wordListResult2.WordListId);
        }
    }
}