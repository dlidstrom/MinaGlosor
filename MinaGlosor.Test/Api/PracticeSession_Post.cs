using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using Castle.Windsor;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Infrastructure;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class PracticeSession_Post : WebApiIntegrationTest
    {
        private IdGenerator generator;
        private HttpResponseMessage result;

        [Test]
        public void CreatesNewPracticeSession()
        {
            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Transact(context =>
                {
                    var practiceSession = context.PracticeSessions.Last();
                    Assert.That(practiceSession, Is.Not.Null);
                    Debug.Assert(practiceSession != null, "practiceSession != null");
                    Assert.That(practiceSession.ValidFrom, Is.EqualTo(new DateTime(2014, 3, 16)));
                    Assert.That(practiceSession.ValidTo, Is.Null);
                    Assert.That(practiceSession.WordListId, Is.EqualTo(2));
                });
        }

        [Test]
        public void ClosesPreviousPracticeSessions()
        {
            // Assert
            Transact(context =>
                {
                    var existing = context.PracticeSessions.First();
                    Assert.That(existing.ValidTo, Is.EqualTo(new DateTime(2014, 3, 16)));
                });
        }

        [Test]
        public void SelectsWordsWithoutScores()
        {
            // Assert
            Transact(context =>
                {
                    var practiceSession = context.PracticeSessions.Last();
                    var practiceWords = practiceSession.PracticeWords;
                    Assert.That(practiceWords.Count, Is.EqualTo(10));
                    foreach (var practiceWord in practiceWords.Take(7))
                    {
                        Assert.That(practiceWord.WordScore.EasynessFactor, Is.EqualTo(0m));
                    }
                });
        }

        [Test]
        public void SelectsWordsWithLowestScore()
        {
            // Assert
            Transact(context =>
            {
                var practiceSession = context.PracticeSessions.Last();
                var practiceWords = practiceSession.PracticeWords;
                Assert.That(practiceWords.Count, Is.EqualTo(10));
                foreach (var practiceWord in practiceWords.Skip(7))
                {
                    Assert.That(practiceWord.WordScore.EasynessFactor, Is.GreaterThan(0m));
                }
            });
        }

        protected override void Act()
        {
            // Arrange
            var vm = new
            {
                wordListId = 2
            };
            SystemTime.UtcDateTime = () => new DateTime(2014, 3, 16);

            // Act
            var response = Client.PostAsJsonAsync("http://temp.uri/api/practicesession", vm);
            result = response.Result;
        }

        protected override void OnSetUp(IWindsorContainer container)
        {
            // Arrange
            generator = new IdGenerator();
            var owner = new User("e@d.com", "pwd") { Id = generator.NextId() };
            Transact(context =>
                {
                    context.Users.Add(owner);

                    // word list with some words that can be selected for practicing
                    // words 1-5, 11-12 have no scores
                    var wordList1 = new WordList("wl1", owner) { Id = generator.NextId() };
                    context.WordLists.Add(wordList1);
                    foreach (var i in Enumerable.Range(0, 12))
                    {
                        var word = wordList1.AddWord("word" + i, "def" + i);
                        word.Id = generator.NextId();
                        context.Words.Add(word);
                    }

                    // the words in range 6-10 should have associated scores
                    foreach (var i in Enumerable.Range(5, 5))
                    {
                        var word = wordList1.Words
                                            .Skip(i)
                                            .First();
                        var wordScore = owner.Score(word);
                        wordScore.Id = generator.NextId();
                        wordScore.UpdateEasynessFactor(5);
                        context.WordScores.Add(wordScore);
                    }

                    var wordList2 = new WordList("wl2", owner) { Id = generator.NextId() };
                    context.WordLists.Add(wordList2);

                    // create an existing practice session of all words except the first 10
                    var practiceWords = new List<PracticeWord>();
                    foreach (var word in wordList1.Words.Skip(10))
                    {
                        var practiceWord = new PracticeWord(new WordScore(owner, word))
                            {
                                Id = generator.NextId()
                            };
                        practiceWords.Add(practiceWord);
                    }

                    var practiceSession = new PracticeSession(wordList1, practiceWords)
                        {
                            Id = generator.NextId()
                        };
                    context.PracticeSessions.Add(practiceSession);
                });

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("e@d.com"), new string[0]);
        }

        protected override void OnTearDown()
        {
            SystemTime.UtcDateTime = null;
        }
    }
}