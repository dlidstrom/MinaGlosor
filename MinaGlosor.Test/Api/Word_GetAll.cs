﻿using System;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using Newtonsoft.Json;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Word_GetAll : WebApiIntegrationTest
    {
        [Test]
        public async void GetsWordsForWordList()
        {
            // Arrange
            var owner = new User("e@d.com", "pwd", "username");
            Transact(session =>
            {
                session.Store(owner);
                var wordList = new WordList("list name", owner);
                session.Store(wordList);

                // make sure listed in date order
                SystemTime.UtcDateTime = () => new DateTime(2012, 1, 1);
                var generator = new KeyGenerator<Word>(session);
                session.Store(new Word(generator.Generate(), "w2", "d2", wordList.Id, Guid.NewGuid(), null));
                SystemTime.UtcDateTime = () => new DateTime(2010, 1, 1);
                session.Store(new Word(generator.Generate(), "w1", "d1", wordList.Id, Guid.NewGuid(), null));
            });

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/word?wordListId=1");
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = await content.ReadAsStringAsync();
            var expected = new
                {
                    wordListName = "list name",
                    words = new[]
                        {
                            new
                                {
                                    id = "2",
                                    text = "w1",
                                    definition = "d1"
                                },
                            new
                                {
                                    id = "1",
                                    text = "w2",
                                    definition = "d2"
                                }
                        }
                };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }
    }
}