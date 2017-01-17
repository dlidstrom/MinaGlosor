using System;
using System.Net;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
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
            User owner;
            Transact(session =>
            {
                owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                session.Store(owner);
            });

            var wordListResponse = await this.PostWordList("wl1");
            await this.PublishWordList(wordListResponse.WordListId, true);
            await this.PostWord("t", "d", wordListResponse.WordListId);
            await this.StartPracticeSession(wordListResponse.WordListId);

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/unfinishedpracticesession?wordListId=" + wordListResponse.WordListId);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            var expected = new
            {
                wordListName = "wl1",
                unfinishedPracticeSessions = new[]
                {
                    new
                    {
                        practiceSessionId = "1",
                        createdDate = "2012-01-01T00:00:00"
                    }
                }
            };
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }
    }
}