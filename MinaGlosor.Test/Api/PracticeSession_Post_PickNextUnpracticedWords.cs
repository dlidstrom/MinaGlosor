using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class PracticeSession_Post_PickNextUnpracticedWords : WebApiIntegrationTest
    {
        [Test]
        public async void NextPracticeSessionUsesUnpracticedWords()
        {
            // Act
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("first@d.com"), new string[0]);
            var request = new
            {
                wordListId = "1"
            };
            var createSessionResponse = await Client.PostAsJsonAsync("http://temp.uri/api/practicesession", request);
            Assert.That(createSessionResponse.Content, Is.Not.Null);
            var createSessionContent = await createSessionResponse.Content.ReadAsAsync<CreateSessionContent>();

            // should be the next 10 words
            Transact(session =>
                {
                    var practiceSession = session.Load<PracticeSession>(PracticeSession.ToId(createSessionContent.PracticeSessionId));
                    Assert.That(practiceSession.Words, Has.Length.EqualTo(10));

                    var expectedWordIds = new HashSet<string>
                        {
                            "words/11",
                            "words/12",
                            "words/13",
                            "words/14",
                            "words/15",
                            "words/16",
                            "words/17",
                            "words/18",
                            "words/19",
                            "words/20"
                        };
                    foreach (var practiceWord in practiceSession.Words)
                    {
                        if (expectedWordIds.Contains(practiceWord.WordId) == false)
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
                    var firstUser = new User("first@d.com", "pwd", "username");
                    session.Store(firstUser);

                    var wordList = new WordList("list", firstUser);
                    session.Store(wordList);

                    // add some words to the word list
                    var currentDate = new DateTime(2012, 1, 1);
                    var generator = new KeyGenerator<Word>(session);
                    for (var i = 0; i < 25; i++)
                    {
                        var newCurrentDate = currentDate.AddSeconds(i);
                        SystemTime.UtcDateTime = () => newCurrentDate;
                        var word = new Word(
                            generator.Generate(),
                            1 + i + "t",
                            1 + i + "d",
                            wordList.Id);
                        session.Store(word);
                    }
                });

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("first@d.com"), new string[0]);

            // mark first word as favourite
            var wordFavouriteResponse = await Client.PostAsJsonAsync("http://temp.uri/api/wordfavourite", new { wordId = 1 });
            Assert.That(wordFavouriteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // practice the first 10
            var request = new
            {
                wordListId = "1"
            };
            var createSessionResponse = await Client.PostAsJsonAsync("http://temp.uri/api/practicesession", request);
            Assert.That(createSessionResponse.Content, Is.Not.Null);
            var createSessionContent = await createSessionResponse.Content.ReadAsAsync<CreateSessionContent>();

            WordConfidenceContent wordConfidenceContent = null;
            for (var i = 0; i < 10; i++)
            {
                var getWordResponse = await Client.GetAsync("http://temp.uri/api/practiceword?practiceSessionId=1");
                Assert.That(getWordResponse.Content, Is.Not.Null);
                var getWordContent = await getWordResponse.Content.ReadAsAsync<GetWordContent>();
                Assert.That(getWordContent.WordId, Is.EqualTo((i + 1).ToString(CultureInfo.InvariantCulture)));
                if (i == 0)
                {
                    Assert.That(getWordContent.IsFavourite, Is.True);
                }
                else
                {
                    Assert.That(getWordContent.IsFavourite, Is.False);
                }

                var postWordConfidenceRequest = new
                {
                    createSessionContent.PracticeSessionId,
                    getWordContent.PracticeWordId,
                    ConfidenceLevel = "PerfectResponse"
                };
                var wordConfidenceResponse = await Client.PostAsJsonAsync(
                    "http://temp.uri/api/wordconfidence", postWordConfidenceRequest);
                Assert.That(wordConfidenceResponse.Content, Is.Not.Null);
                wordConfidenceContent = await wordConfidenceResponse.Content.ReadAsAsync<WordConfidenceContent>();

                if (wordConfidenceContent.IsFinished) break;
            }

            Assert.That(wordConfidenceContent, Is.Not.Null);
            Debug.Assert(wordConfidenceContent != null, "wordConfidenceContent != null");
            Assert.That(wordConfidenceContent.IsFinished, Is.True, "Expected practice session to finish after 10 steps");
        }

        public class CreateSessionContent
        {
            public string PracticeSessionId { get; set; }
        }

        public class GetWordContent
        {
            public string PracticeWordId { get; set; }

            public string WordId { get; set; }

            public bool IsFavourite { get; set; }
        }

        public class WordConfidenceContent
        {
            public bool IsFinished { get; set; }
        }
    }
}