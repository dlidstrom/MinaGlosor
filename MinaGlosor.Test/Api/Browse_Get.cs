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
    public class Browse_Get : WebApiIntegrationTest
    {
        [Test]
        public async void GetsAllWordLists()
        {
            // Arrange
            Transact(session =>
            {
                // fortsätt här
                session.Store(new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username1", UserRole.Admin));
                session.Store(new User(KeyGeneratorBase.Generate<User>(session), "f@d.com", "pwd", "username2", UserRole.Admin));
            });

            // belongs to e@d.com
            await this.PostWordList("list1");
            var publishedWordListResponse1 = await this.PostWordList("list2");
            await this.PublishWordList(publishedWordListResponse1.WordListId, true);

            // belongs to f@d.com
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("f@d.com"), new string[0]);
            var publishedWordListResponse2 = await this.PostWordList("list3");
            await this.PublishWordList(publishedWordListResponse2.WordListId, true);

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/browse");
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
                        wordListId = publishedWordListResponse1.WordListId,
                        name = "list2",
                        numberOfWords = 0,
                        username = "username1",
                        gravatarHash = "e528f7e2efd2431e5fa05859ee474df8"
                    },
                    new
                    {
                        wordListId = publishedWordListResponse2.WordListId,
                        name = "list3",
                        numberOfWords = 0,
                        username = "username2",
                        gravatarHash = "e84879df1fe98a8cb559cf7ee65eb16f"
                    }
                },
                paging = new
                {
                    totalItems = 2,
                    currentPage = 1,
                    itemsPerPage = 50,
                    hasPages = false
                }
            };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }
    }
}