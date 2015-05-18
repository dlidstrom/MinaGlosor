﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MinaGlosor.Web.Models;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class PracticeSession_Post_WhenExisting : WebApiIntegrationTest
    {
        private HttpResponseMessage response;
        private ExpectedContent content;

        [Test]
        public void AddPracticedWordsToPracticeSessionFollowedByNewWords()
        {
            // Assert
            PracticeSession practiceSession = null;
            Transact(session =>
                {
                    practiceSession = session.Load<PracticeSession>(PracticeSession.ToId(content.PracticeSessionId));
                });

            var words = new HashSet<string>(practiceSession.Words.Select(x => x.WordId));
            Assert.That(words, Has.Count.EqualTo(10));
            Assert.That(words, Contains.Item("words/1"));
            Assert.That(words, Contains.Item("words/2"));
            Assert.That(words, Contains.Item("words/3"));
            Assert.That(words, Contains.Item("words/4"));
            Assert.That(words, Contains.Item("words/5"));
            Assert.That(words, Contains.Item("words/16"));
            Assert.That(words, Contains.Item("words/17"));
            Assert.That(words, Contains.Item("words/18"));
            Assert.That(words, Contains.Item("words/19"));
            Assert.That(words, Contains.Item("words/20"));
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

                    // last should be practiced already (these should be selected)
                    for (var i = 15; i < 20; i++)
                    {
                        var newCurrentDate = currentDate.AddSeconds(i);
                        SystemTime.UtcDateTime = () => newCurrentDate;
                        var word = new Word(
                            "Words/" + i,
                            1 + i + "t",
                            1 + i + "d",
                            wordList.Id,
                            Guid.NewGuid(),
                            null);
                        session.Store(word);
                        var wordScore = new WordScore(owner.Id, word.Id, wordList.Id);
                        wordScore.ScoreWord(ConfidenceLevel.PerfectResponse);
                        session.Store(wordScore);
                    }

                    // add some practice word that is for the far future (this should not be selected)
                    var futureWord = new Word(
                        "Words/21",
                        "future",
                        "future",
                        wordList.Id,
                        Guid.NewGuid(),
                        null);
                    session.Store(futureWord);
                    var futureWordScore = new WordScore(owner.Id, futureWord.Id, wordList.Id);
                    futureWordScore.ScoreWord(ConfidenceLevel.PerfectResponse);
                    SystemTime.UtcDateTime = () => new DateTime(2012, 1, 2, 1, 0, 0);
                    futureWordScore.ScoreWord(ConfidenceLevel.PerfectResponse);
                    session.Store(futureWordScore);
                });

            SystemTime.UtcDateTime = () => new DateTime(2012, 1, 3);

            // Act
            var request = new
                {
                    wordListId = "1"
                };
            response = await Client.PostAsJsonAsync("http://temp.uri/api/practicesession", request);
            Assert.That(response.Content, Is.Not.Null);
            content = await response.Content.ReadAsAsync<ExpectedContent>();
        }

        public class ExpectedContent
        {
            public string PracticeSessionId { get; set; }
        }
    }
}