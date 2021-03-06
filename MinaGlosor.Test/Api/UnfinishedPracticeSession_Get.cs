﻿using System;
using System.Linq;
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
            Transact(session =>
                {
                    var owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                    session.Store(owner);
                    var wordList = new WordList(KeyGeneratorBase.Generate<WordList>(session), "wl1", owner.Id);
                    session.Store(wordList);
                    var generator = new KeyGenerator<Word>(session);
                    var practiceWords = Enumerable.Range(1, 10).Select(i =>
                        {
                            var word = Word.Create(
                                generator.Generate(),
                                "t" + i,
                                "d" + i,
                                wordList);
                            session.Store(word);
                            var practiceWord = new PracticeWord(word, wordList.Id, owner.Id);
                            return practiceWord;
                        }).ToArray();
                    session.Store(new PracticeSession(KeyGeneratorBase.Generate<PracticeSession>(session), wordList.Id, practiceWords, owner.Id));
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