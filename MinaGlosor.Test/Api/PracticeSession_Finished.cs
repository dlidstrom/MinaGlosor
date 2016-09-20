using System.Globalization;
using System.Net.Http;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class PracticeSession_Finished : WebApiIntegrationTest
    {
        private HttpContent content;

        [Test]
        public async void SignalsDifficultWord()
        {
            // Assert
            Assert.That(content, Is.Not.Null);
            var result = await content.ReadAsStringAsync();
            var expected = new
            {
                numberOfFavourites = 0,
                progresses = new[]
                        {
                            new
                                {
                                    wordListId = "1",
                                    ownerId = "1",
                                    name = "list",
                                    numberOfWords = 2,
                                    percentDone = 100,
                                    percentExpired = 0,
                                    percentDifficultWords = 50,
                                    numberOfDifficultWords = 1,
                                    published = false,
                                    gravatarHash = "e528f7e2efd2431e5fa05859ee474df8"
                                }
                        },
                paging = new
                {
                    totalItems = 2,
                    currentPage = 1,
                    itemsPerPage = 20,
                    hasPages = false
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
                session.Store(owner);
            });

            var postWordListResponse = await this.PostWordList("list");

            // add some words to the word list
            for (var i = 0; i < 2; i++)
            {
                await this.PostWord(
                    i.ToString(CultureInfo.InvariantCulture),
                    i.ToString(CultureInfo.InvariantCulture),
                    postWordListResponse.WordListId);
            }

            // Act
            var practiceSessionResponse = await this.StartPracticeSession(postWordListResponse.WordListId);

            var responses = new[] { ConfidenceLevel.PerfectResponse, ConfidenceLevel.PerfectResponse };
            for (int i = 0; i < 2; i++)
            {
                // get next practice word
                var practiceWordResponse = await this.GetNextPracticeWord(practiceSessionResponse.PracticeSessionId);

                // post word confidence
                await this.PostWordConfidence(practiceSessionResponse.PracticeSessionId, practiceWordResponse.PracticeWordId, responses[i]);
            }

            var response = await Client.GetAsync("http://temp.uri/api/progress");
            content = response.Content;
        }
    }
}