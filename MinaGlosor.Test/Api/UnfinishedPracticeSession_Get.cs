using System;
using System.Linq;
using System.Net;
using MinaGlosor.Web.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class UnfinishedPracticeSession_Get : WebApiIntegrationTest
    {
        [Test]
        public async void ReturnsUnfinishedPracticeSessions()
        {
            // Arrange
            SystemTime.UtcDateTime = () => new DateTime(2012, 1, 1);
            Transact(session =>
                {
                    var owner = new User("e@d.com", "pwd", "username");
                    session.Store(owner);
                    var wordList = new WordList("wl1", owner);
                    session.Store(wordList);
                    var practiceWords = Enumerable.Range(1, 10).Select(i =>
                        {
                            var word = new Word(
                                "Words/" + i,
                                "t" + i,
                                "d" + i,
                                wordList.Id,
                                Guid.NewGuid(),
                                null);
                            session.Store(word);
                            var practiceWord = new PracticeWord(word, wordList.Id, owner.Id);
                            return practiceWord;
                        }).ToArray();
                    session.Store(new PracticeSession(wordList.Id, practiceWords, "users/1"));
                });

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/unfinishedpracticesession?wordListId=1");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            var expected = new[]
                {
                    new
                        {
                            practiceSessionId = "1",
                            createdDate = "2012-01-01T00:00:00"
                        }
                };
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }
    }
}