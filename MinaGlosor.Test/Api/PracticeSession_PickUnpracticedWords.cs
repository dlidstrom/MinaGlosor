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
    public class PracticeSession_PickUnpracticedWords : WebApiIntegrationTest
    {
        [Test]
        public async void PicksUnpracticedWords()
        {
            // Act
            SystemTime.UtcDateTime = () => new DateTime(2012, 1, 3);
            WaitForIndexing();
            var createSessionResponse = await this.StartPracticeSession("1", PracticeMode.UnpracticedPreferred);

            Transact(session =>
            {
                var practiceSession = session.Load<PracticeSession>(PracticeSession.ToId(createSessionResponse.PracticeSessionId));

                var expectedWordIds = new[]
                {
                    "21",
                    "25",
                    "29",
                    "33",
                    "37"
                };
                Assert.That(practiceSession.Words.Select(x => Word.FromId(x.WordId)), Is.EqualTo(expectedWordIds));
            });
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
            SystemTime.UtcDateTime = () => new DateTime(2012, 1, 1);
            for (var i = 0; i < 5; i++)
            {
                await this.PostWord(
                    1 + i + "t",
                    1 + i + "d",
                    wordListResponse.WordListId);
            }

            // practice 5
            var createSessionResponse = await this.StartPracticeSession(wordListResponse.WordListId);

            WordConfidenceExtensions.Response wordConfidenceResponse = null;
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

            // add 5 more
            for (var i = 5; i < 10; i++)
            {
                await this.PostWord(
                    1 + i + "t",
                    1 + i + "d",
                    wordListResponse.WordListId);
            }
        }
    }
}