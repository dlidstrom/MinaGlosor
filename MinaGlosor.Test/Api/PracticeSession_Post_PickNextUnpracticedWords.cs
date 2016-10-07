using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class PracticeSession_Post_PickNextUnpracticedWords : WebApiIntegrationTest
    {
        private readonly List<string> wordIds = new List<string>();

        [Test]
        public async void NextPracticeSessionUsesUnpracticedWords()
        {
            // Act
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("first@d.com"), new string[0]);
            var createSessionResponse = await this.StartPracticeSession("1");

            // should be the next 10 words
            TracingLogger.Information("Verifying expected words");
            Transact(session =>
                {
                    var practiceSession = session.Load<PracticeSession>(PracticeSession.ToId(createSessionResponse.PracticeSessionId));
                    Assert.That(practiceSession.Words, Has.Length.EqualTo(10));

                    var expectedWordIds = new HashSet<string>(wordIds.Skip(10).Take(10));
                    foreach (var practiceWord in practiceSession.Words)
                    {
                        if (expectedWordIds.Contains(Word.FromId(practiceWord.WordId)) == false)
                            Assert.Fail("{0} was not expected", practiceWord.WordId);
                    }
                });
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
                var firstUser = new User(KeyGeneratorBase.Generate<User>(session), "first@d.com", "pwd", "username");
                session.Store(firstUser);
            });

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("first@d.com"), new string[0]);

            var wordListResponse = await this.PostWordList();

            // add some words to the word list
            var currentDate = new DateTime(2012, 1, 1);
            for (var i = 0; i < 25; i++)
            {
                var newCurrentDate = currentDate.AddSeconds(i);
                SystemTime.UtcDateTime = () => newCurrentDate;
                var wordResponse = await this.PostWord(
                    1 + i + "t",
                    1 + i + "d",
                    wordListResponse.WordListId);
                wordIds.Add(wordResponse.WordId);
            }

            // mark first word as favourite
            await this.MarkWordAsFavourite("1", true);

            // practice the first 10
            var createSessionResponse = await this.StartPracticeSession(wordListResponse.WordListId);

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
                    ConfidenceLevel.PerfectResponse);

                if (wordConfidenceResponse.IsFinished) break;
            }

            Assert.That(wordConfidenceResponse, Is.Not.Null);
            Debug.Assert(wordConfidenceResponse != null, "wordConfidenceResponse != null");
            Assert.That(wordConfidenceResponse.IsFinished, Is.True, "Expected practice session to finish after 10 steps");
        }
    }
}