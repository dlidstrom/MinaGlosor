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
                        session.Store(new Word(1 + i + "t", 1 + i + "d", wordList.Id));
                    }
                });

            var request = new
                {
                    wordListId = "1"
                };
            await Client.PostAsJsonAsync("http://temp.uri/api/practicesession", request);

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
            Assert.That(content.WordListName, Is.EqualTo("list"));
            Assert.That(content.Text, Is.EqualTo("1t"));
            Assert.That(content.Definition, Is.EqualTo("1d"));
            Assert.That(content.PracticeWordId, Is.EqualTo(expectedPracticeWordId));
        }

        public class ExpectedContent
        {
            public string Text { get; set; }

            public string Definition { get; set; }

            public string PracticeWordId { get; set; }

            public string PracticeSessionId { get; set; }

            public string WordListName { get; set; }
        }
    }
}