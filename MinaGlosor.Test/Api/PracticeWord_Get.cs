using System;
using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Models;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class PracticeWord_Get : WebApiIntegrationTest
    {
        [Test]
        public async void ReturnsFirstWordForPractice()
        {
            string expectedPracticeWordId = null;
            Transact(session =>
                {
                    expectedPracticeWordId = session.Load<PracticeSession>("PracticeSessions/1").Words[0].PracticeWordId;
                });

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/practiceword?practiceSessionId=1");
            Assert.That(response.Content, Is.Not.Null);
            var content = await response.Content.ReadAsAsync<ExpectedContent>();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content.PracticeSessionId, Is.EqualTo("1"));
            Assert.That(content.WordListId, Is.EqualTo("1"));
            Assert.That(content.WordListName, Is.EqualTo("list"));
            Assert.That(content.Text, Is.EqualTo("1t"));
            Assert.That(content.Definition, Is.EqualTo("1d"));
            Assert.That(content.PracticeWordId, Is.EqualTo(expectedPracticeWordId));
            Assert.That(content.Green, Is.EqualTo(0));
            Assert.That(content.Blue, Is.EqualTo(0));
            Assert.That(content.Yellow, Is.EqualTo(0));
        }

        [Test]
        public async void GetsById()
        {
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
                ConfidenceLevel = "RecalledWithSeriousDifficulty"
            };
            await Client.PostAsJsonAsync("http://temp.uri/api/wordconfidence", postWordConfidenceRequest);
            var response = await Client.GetAsync("http://temp.uri/api/practiceword?practiceSessionId=1&practiceWordId=" + practiceWordId);
            Assert.That(response.Content, Is.Not.Null);
            var content = await response.Content.ReadAsAsync<ExpectedContent>();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content.PracticeSessionId, Is.EqualTo("1"));
            Assert.That(content.WordListId, Is.EqualTo("1"));
            Assert.That(content.WordListName, Is.EqualTo("list"));
            Assert.That(content.Text, Is.EqualTo("1t"));
            Assert.That(content.Definition, Is.EqualTo("1d"));
            Assert.That(content.PracticeWordId, Is.EqualTo(practiceWordId));
            Assert.That(content.Green, Is.EqualTo(0));
            Assert.That(content.Blue, Is.EqualTo(10));
            Assert.That(content.Yellow, Is.EqualTo(0));
        }

        protected override void Arrange()
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
                for (var i = 0; i < 15; i++)
                {
                    var newCurrentDate = currentDate.AddSeconds(i);
                    SystemTime.UtcDateTime = () => newCurrentDate;
                    var word = new Word(
                        "Words/" + (1 + i),
                        1 + i + "t",
                        1 + i + "d",
                        wordList.Id,
                        Guid.NewGuid(),
                        null);
                    session.Store(word);
                }
            });

            var request = new
            {
                wordListId = "1"
            };
            var result = Client.PostAsJsonAsync("http://temp.uri/api/practicesession", request).Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        public class ExpectedContent
        {
            public string Text { get; set; }

            public string Definition { get; set; }

            public string PracticeWordId { get; set; }

            public string PracticeSessionId { get; set; }

            public string WordListId { get; set; }

            public string WordListName { get; set; }

            public int Green { get; set; }

            public int Blue { get; set; }

            public int Yellow { get; set; }
        }
    }
}