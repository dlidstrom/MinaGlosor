using System.Security.Principal;
using System.Threading;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class PracticeSession_Post_OtherUsers : WebApiIntegrationTest
    {
        [Test]
        public async void CreatesTheProgress()
        {
            // Arrange
            Transact(session =>
            {
                var owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username1");
                session.Store(owner);

                var actor = new User(KeyGeneratorBase.Generate<User>(session), "f@d.com", "pwd", "username2");
                session.Store(actor);
            });

            // belongs to e@d.com
            var wordListResponse = await this.PostWordList("list");
            await this.PublishWordList(wordListResponse.WordListId, true);
            await this.PostWord("text", "def", wordListResponse.WordListId);

            // switch current user
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("f@d.com"), new string[0]);

            // Act
            await this.StartPracticeSession(wordListResponse.WordListId);

            // Assert
            var response = await Client.GetAsync("http://temp.uri/api/progress");
            var content = await response.Content.ReadAsStringAsync();
            var expected = new
            {
                numberOfFavourites = 0,
                progresses = new[]
                {
                    new
                    {
                        progressId = "1",
                        wordListId = "1",
                        progressOwnerId = "5",
                        wordListOwnerId = "1",
                        wordListOwnerUsername = "username1",
                        isBorrowedWordList = true,
                        name = "list",
                        numberOfWords = 1,
                        percentDone = 0,
                        numberOfWordsExpired = 0,
                        percentExpired = 0,
                        numberOfEasyWords = 0,
                        percentEasyWords = 0,
                        numberOfDifficultWords = 0,
                        percentDifficultWords = 0,
                        published = false,
                        gravatarHash = "e84879df1fe98a8cb559cf7ee65eb16f"
                    }
                },
                paging = new
                {
                    totalItems = 1,
                    currentPage = 1,
                    itemsPerPage = 20,
                    hasPages = false
                }
            };
            Assert.That(content, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }
    }
}