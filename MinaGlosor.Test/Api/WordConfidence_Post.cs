using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using Newtonsoft.Json;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordConfidence_Post : WebApiIntegrationTest
    {
        [Test]
        public async void UpdatesWordScore()
        {
            // Arrange
            Transact(session =>
                {
                    var owner = new User("e@d.com", "pwd", "username");
                    session.Store(owner);

                    var wordList = new WordList("list", owner);
                    session.Store(wordList);

                    // add some words to the word list
                    var currentDate = new DateTime(2012, 1, 1);
                    var generator = new KeyGenerator<Word>(session);
                    for (var i = 0; i < 15; i++)
                    {
                        var newCurrentDate = currentDate.AddSeconds(i);
                        SystemTime.UtcDateTime = () => newCurrentDate;
                        session.Store(new Word(generator.Generate(), 1 + i + "t", 1 + i + "d", wordList.Id, Guid.NewGuid(), null));
                    }
                });

            var createPracticeSessionRequest = new
                {
                    wordListId = "1"
                };
            await Client.PostAsJsonAsync("http://temp.uri/api/practicesession", createPracticeSessionRequest);

            string practiceWordId = null;
            Transact(session =>
                {
                    practiceWordId = session.Load<PracticeSession>("PracticeSessions/1").Words[0].PracticeWordId;
                });

            // Act
            var postWordConfidenceRequest = new
                {
                    PracticeSessionId = "1",
                    PracticeWordId = practiceWordId,
                    ConfidenceLevel = "PerfectResponse"
                };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/wordconfidence", postWordConfidenceRequest);
            Assert.That(response.Content, Is.Not.Null);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            var expectedResponse = new
                {
                    isFinished = false,
                    green = 10,
                    blue = 0,
                    yellow = 0
                };
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(content, Is.EqualTo(JsonConvert.SerializeObject(expectedResponse)));
            Transact(session =>
                {
                    var practiceSession = session.Load<PracticeSession>("PracticeSessions/1");
                    var word = practiceSession.Words.SingleOrDefault(x => x.PracticeWordId == practiceWordId);
                    Assert.That(word, Is.Not.Null);
                    Debug.Assert(word != null, "word != null");
                    Assert.That(word.Confidence, Is.EqualTo((int)ConfidenceLevel.PerfectResponse));
                });
        }
    }
}