using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Progress_Get_Favourite : WebApiIntegrationTest
    {
        [Test]
        public async void GetsWithFavourite()
        {
            // Act
            var response = await Client.GetAsync("http://temp.uri/api/progress");
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = await content.ReadAsStringAsync();
            var expected = new
                {
                    numberOfFavourites = 1,
                    progresses = new[]
                        {
                            new
                                {
                                    wordListId = "1",
                                    ownerId = "1",
                                    name = "list",
                                    numberOfWords = 10,
                                    percentDone = 0,
                                    percentExpired = 0
                                }
                        },
                    paging = new
                    {
                        totalItems = 1,
                        currentPage = 1,
                        itemsPerPage = 20
                    }
                };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        protected override async void Act()
        {
            // Arrange
            Transact(session =>
            {
                var owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                var anotherUser = new User(KeyGeneratorBase.Generate<User>(session), "f@d.com", "pwd", "username");
                session.Store(owner);
                session.Store(anotherUser);
            });

            var wordListResponse = await this.PostWordList("list");

            // add some words to the word list
            var wordResponse = await this.PostWord(1 + "t", 1 + "d", wordListResponse.WordListId);
            for (var i = 1; i < 10; i++)
            {
                await this.PostWord(1 + i + "t", 1 + i + "d", wordListResponse.WordListId);
            }

            // store favourite for another user
            await this.MarkWordAsFavourite(wordResponse.WordId, true);
        }
    }
}