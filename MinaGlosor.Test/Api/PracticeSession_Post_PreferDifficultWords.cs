using System;
using System.Diagnostics;
using System.Linq;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class PracticeSession_Post_PreferDifficultWords : WebApiIntegrationTest
    {
        [Test]
        public async void NextPracticeSessionUsesUnpracticedWords()
        {
            // Act
            SystemTime.UtcDateTime = () => new DateTime(2012, 1, 3);
            WaitForIndexing();
            var createSessionResponse = await this.StartPracticeSession("1");

            // should be the next 10 words
            Transact(session =>
            {
                var practiceSession = session.Load<PracticeSession>(PracticeSession.ToId(createSessionResponse.PracticeSessionId));

                var expectedWordIds = new[]
                {
                    "21",
                    "25",
                    "29",
                    "33",
                    "37",
                    "1",
                    "5",
                    "9",
                    "13",
                    "17"
                };
                Assert.That(practiceSession.Words.Select(x => Word.FromId(x.WordId)), Is.EqualTo(expectedWordIds));
            });
        }

        protected override void OnTearDown()
        {
            SystemTime.UtcDateTime = null;
        }

        protected override async void Arrange()
        {
            // Arrange
            Transact(session =>
            {
                var firstUser = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                session.Store(firstUser);
            });

            var wordListResponse = await this.PostWordList();
            await this.PublishWordList(wordListResponse.WordListId, true);

            // add some words to the word list
            var currentDate = new DateTime(2012, 1, 1);
            for (var i = 0; i < 10; i++)
            {
                var newCurrentDate = currentDate.AddSeconds(i);
                SystemTime.UtcDateTime = () => newCurrentDate;
                await this.PostWord(
                    1 + i + "t",
                    1 + i + "d",
                    wordListResponse.WordListId);
            }

            // mark first word as favourite
            await this.MarkWordAsFavourite("1", true);

            // practice the first 10
            var createSessionResponse = await this.StartPracticeSession(wordListResponse.WordListId);

            var responses = new[]
            {
                ConfidenceLevel.PerfectResponse,
                ConfidenceLevel.PerfectResponse,
                ConfidenceLevel.PerfectResponse,
                ConfidenceLevel.PerfectResponse,
                ConfidenceLevel.PerfectResponse,
                ConfidenceLevel.RecalledWithSeriousDifficulty,
                ConfidenceLevel.RecalledWithSeriousDifficulty,
                ConfidenceLevel.RecalledWithSeriousDifficulty,
                ConfidenceLevel.RecalledWithSeriousDifficulty,
                ConfidenceLevel.RecalledWithSeriousDifficulty
            };
            WordConfidenceExtensions.Response wordConfidenceResponse = null;
            for (var i = 0; i < 10; i++)
            {
                var getWordResponse = await this.GetNextPracticeWord(createSessionResponse.PracticeSessionId);
                if (i == 0)
                {
                    Assert.That(getWordResponse.IsFavourite, Is.True);
                }
                else
                {
                    Assert.That(getWordResponse.IsFavourite, Is.False);
                }

                wordConfidenceResponse = await this.PostWordConfidence(
                    createSessionResponse.PracticeSessionId,
                    getWordResponse.PracticeWordId,
                    responses[i]);
            }

            // finish off
            for (var i = 0; i < 5; i++)
            {
                var getWordResponse = await this.GetNextPracticeWord(createSessionResponse.PracticeSessionId);
                wordConfidenceResponse = await this.PostWordConfidence(
                    createSessionResponse.PracticeSessionId,
                    getWordResponse.PracticeWordId,
                    ConfidenceLevel.PerfectResponse);
            }

            Assert.That(wordConfidenceResponse, Is.Not.Null);
            Debug.Assert(wordConfidenceResponse != null, "wordConfidenceResponse != null");
            Assert.That(wordConfidenceResponse.IsFinished, Is.True, "Expected practice session to finish after 10 steps");
        }
    }
}