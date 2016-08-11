using System;
using System.Net.Http;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Controllers.Api;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using Newtonsoft.Json;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordList_Get_Percent : WebApiIntegrationTest
    {
        [Test]
        public async void GetsWithPercentComplete()
        {
            // Act
            SystemTime.UtcDateTime = () => new DateTime(2012, 1, 3, 0, 0, 0);
            WaitForIndexing();
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
                                    name = "list",
                                    numberOfWords = 10,
                                    percentDone = 0,
                                    percentExpired = 30
                                }
                        },
                    numberOfFavourites = 0
                };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        protected override async void Act()
        {
            // Arrange
            SystemTime.UtcDateTime = () => new DateTime(2012, 1, 1);
            Transact(session =>
            {
                var owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                session.Store(owner);
            });

            var wordListResponse = await this.PostWordList("list");

            // add some words to the word list
            for (var i = 0; i < 10; i++)
            {
                await this.PostWord(1 + i + "t", 1 + i + "d", wordListResponse.WordListId);
            }

            // practice the first word
            var request = new
            {
                wordListId = "1"
            };
            var createSessionResponse = await Client.PostAsJsonAsync("http://temp.uri/api/practicesession", request);
            Assert.That(createSessionResponse.Content, Is.Not.Null);
            var createSessionContent = await createSessionResponse.Content.ReadAsAsync<CreateSessionContent>();

            var secondsToAdd = new [] { 0, 10, 20 };
            var answers = new[] { "PerfectResponse", "PerfectResponse", "IncorrectWithEasyRecall" };
            for (var i = 0; i < 3; i++)
            {
                var second = secondsToAdd[i];
                var answer = answers[i];
                SystemTime.UtcDateTime = () => new DateTime(2012, 1, 1, 0, 0, second);
                var getWordResponse1 = await Client.GetAsync("http://temp.uri/api/practiceword?practiceSessionId=" + createSessionContent.PracticeSessionId);
                Assert.That(getWordResponse1.Content, Is.Not.Null);
                var getWordContent = await getWordResponse1.Content.ReadAsAsync<GetWordContent>();

                var postWordConfidenceRequest = new WordConfidenceController.WordConfidenceRequest(
                    createSessionContent.PracticeSessionId,
                    getWordContent.PracticeWordId,
                    answer);
                var wordConfidenceResponse = await Client.PostAsJsonAsync(
                    "http://temp.uri/api/wordconfidence", postWordConfidenceRequest);
                Assert.That(wordConfidenceResponse.Content, Is.Not.Null);
                var wordConfidenceContent = await wordConfidenceResponse.Content.ReadAsAsync<WordConfidenceContent>();
                Assert.That(wordConfidenceContent.IsFinished, Is.False);
            }
        }

        public class CreateSessionContent
        {
            public string PracticeSessionId { get; set; }
        }

        public class WordConfidenceContent
        {
            public bool IsFinished { get; set; }
        }

        public class GetWordContent
        {
            public string PracticeWordId { get; set; }
        }
    }
}