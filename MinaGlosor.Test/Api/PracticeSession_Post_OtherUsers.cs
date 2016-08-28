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
        public async void CreatesTheWordListProgress()
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
            await this.PostWord("text", "def", wordListResponse.WordListId);

            // switch current user
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("f@d.com"), new string[0]);

            // Act
            await this.StartPracticeSession(wordListResponse.WordListId);

            // Assert
            var response = await Client.GetAsync("http://temp.uri/api/wordlist");
            var content = await response.Content.ReadAsStringAsync();
            var expected = new
            {
                wordLists = new[]
                {
                    new
                    {
                        wordListId = "1",
                        ownerId = "5",
                        name = "list",
                        numberOfWords = 1,
                        percentDone = 0,
                        percentExpired = 0
                    }
                },
                numberOfFavourites = 0
            };
            Assert.That(content, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }
    }
}