using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using Newtonsoft.Json;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class PracticeSession_Finished : WebApiIntegrationTest
    {
        private HttpContent content;
        private PracticeSessionExtensions.PracticeSessionResponse practiceSessionResponse;
        private WordListExtensions.PostWordListResponse postWordListResponse;

        [Test]
        public async void OneEasyRestDifficult()
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
                                    progressId = "1",
                                    wordListId = "1",
                                    progressOwnerId = "1",
                                    wordListOwnerId = "1",
                                    wordListOwnerUsername = "username",
                                    isBorrowedWordList = false,
                                    name = "list",
                                    numberOfWords = 3,
                                    percentDone = 100,
                                    numberOfWordsExpired = 0,
                                    percentExpired = 0,
                                    numberOfEasyWords = 2,
                                    percentEasyWords = 67,
                                    numberOfDifficultWords = 1,
                                    percentDifficultWords = 33,
                                    published = true,
                                    gravatarHash = "e528f7e2efd2431e5fa05859ee474df8"
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
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        [Test]
        public async void AllHard()
        {
            // Act
            SystemTime.UtcDateTime = () => new DateTime(2016, 1, 2, 0, 0, 1);
            practiceSessionResponse = await this.StartPracticeSession(postWordListResponse.WordListId);

            PracticeSessionExtensions.PracticeWordResponse practiceWordResponse;
            WordConfidenceExtensions.Response wordConfidenceResponse = null;
            for (int i = 0; i < 3; i++)
            {
                practiceWordResponse = await this.GetNextPracticeWord(practiceSessionResponse.PracticeSessionId);
                await this.PostWordConfidence(
                    practiceSessionResponse.PracticeSessionId,
                    practiceWordResponse.PracticeWordId,
                    ConfidenceLevel.RecalledWithSeriousDifficulty);
            }

            // finish off
            for (int i = 0; i < 3; i++)
            {
                practiceWordResponse = await this.GetNextPracticeWord(practiceSessionResponse.PracticeSessionId);
                wordConfidenceResponse = await this.PostWordConfidence(
                    practiceSessionResponse.PracticeSessionId,
                    practiceWordResponse.PracticeWordId,
                    ConfidenceLevel.PerfectResponse);
            }

            Debug.Assert(wordConfidenceResponse != null, "wordConfidenceResponse != null");
            Assert.That(wordConfidenceResponse.IsFinished);

            var response = await Client.GetAsync("http://temp.uri/api/progress");
            content = response.Content;

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
                                    progressId = "1",
                                    wordListId = "1",
                                    progressOwnerId = "1",
                                    wordListOwnerId = "1",
                                    wordListOwnerUsername = "username",
                                    isBorrowedWordList = false,
                                    name = "list",
                                    numberOfWords = 3,
                                    percentDone = 100,
                                    numberOfWordsExpired = 0,
                                    percentExpired = 0,
                                    numberOfEasyWords = 0,
                                    percentEasyWords = 0,
                                    numberOfDifficultWords = 3,
                                    percentDifficultWords = 100,
                                    published = true,
                                    gravatarHash = "e528f7e2efd2431e5fa05859ee474df8"
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
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        [Test]
        public async void AllEasy()
        {
            // Act
            SystemTime.UtcDateTime = () => new DateTime(2016, 1, 2, 0, 0, 1);
            practiceSessionResponse = await this.StartPracticeSession(postWordListResponse.WordListId);

            WordConfidenceExtensions.Response wordConfidenceResponse = null;
            for (int i = 0; i < 3; i++)
            {
                var practiceWordResponse = await this.GetNextPracticeWord(practiceSessionResponse.PracticeSessionId);
                wordConfidenceResponse = await this.PostWordConfidence(
                    practiceSessionResponse.PracticeSessionId,
                    practiceWordResponse.PracticeWordId,
                    ConfidenceLevel.PerfectResponse);
            }

            Debug.Assert(wordConfidenceResponse != null, "wordConfidenceResponse != null");
            Assert.That(wordConfidenceResponse.IsFinished);

            var response = await Client.GetAsync("http://temp.uri/api/progress");
            content = response.Content;

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
                                    progressId = "1",
                                    wordListId = "1",
                                    progressOwnerId = "1",
                                    wordListOwnerId = "1",
                                    wordListOwnerUsername = "username",
                                    isBorrowedWordList = false,
                                    name = "list",
                                    numberOfWords = 3,
                                    percentDone = 100,
                                    numberOfWordsExpired = 0,
                                    percentExpired = 0,
                                    numberOfEasyWords = 3,
                                    percentEasyWords = 100,
                                    numberOfDifficultWords = 0,
                                    percentDifficultWords = 0,
                                    published = true,
                                    gravatarHash = "e528f7e2efd2431e5fa05859ee474df8"
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
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        protected override void Arrange()
        {
            SystemTime.UtcDateTime = () => new DateTime(2016, 1, 1, 0, 0, 0);
        }

        protected override void OnTearDown()
        {
            SystemTime.UtcDateTime = null;
        }

        protected override async void Act()
        {
            // Arrange
            Transact(session =>
            {
                var owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                session.Store(owner);
            });

            postWordListResponse = await this.PostWordList("list");
            await this.PublishWordList(postWordListResponse.WordListId, true);

            // add some words to the word list
            for (var i = 0; i < 3; i++)
            {
                await this.PostWord(
                    i.ToString(CultureInfo.InvariantCulture),
                    i.ToString(CultureInfo.InvariantCulture),
                    postWordListResponse.WordListId);
            }

            // Act
            practiceSessionResponse = await this.StartPracticeSession(postWordListResponse.WordListId);

            var confidenceLevels = new[]
            {
                ConfidenceLevel.RecalledWithSeriousDifficulty,
                ConfidenceLevel.PerfectResponse,
                ConfidenceLevel.PerfectResponse
            };
            PracticeSessionExtensions.PracticeWordResponse practiceWordResponse;
            WordConfidenceExtensions.Response wordConfidenceResponse;
            foreach (var confidenceLevel in confidenceLevels)
            {
                // get next practice word
                practiceWordResponse = await this.GetNextPracticeWord(practiceSessionResponse.PracticeSessionId);

                // post word confidence
                wordConfidenceResponse = await this.PostWordConfidence(
                    practiceSessionResponse.PracticeSessionId,
                    practiceWordResponse.PracticeWordId,
                    confidenceLevel);
                Assert.That(wordConfidenceResponse.IsFinished, Is.False);
            }

            // finish off
            // get next practice word
            practiceWordResponse = await this.GetNextPracticeWord(practiceSessionResponse.PracticeSessionId);
            wordConfidenceResponse = await this.PostWordConfidence(
                practiceSessionResponse.PracticeSessionId,
                practiceWordResponse.PracticeWordId,
                ConfidenceLevel.PerfectResponse);
            Assert.That(wordConfidenceResponse.IsFinished);

            var response = await Client.GetAsync("http://temp.uri/api/progress");
            response.EnsureSuccessStatusCode();
            content = response.Content;
        }
    }
}