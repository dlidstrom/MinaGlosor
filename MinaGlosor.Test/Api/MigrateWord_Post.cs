using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using MinaGlosor.Web.Models.Indexes;
using NUnit.Framework;
using Raven.Client.Linq;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class MigrateWord_Post : WebApiIntegrationTest
    {
        [Test]
        public async void CreatesTheWord()
        {
            // Arrange
            string wordListId = null;
            Transact(session =>
                {
                    var user = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username", UserRole.Admin);
                    session.Store(user);
                    var wordList = new WordList(KeyGeneratorBase.Generate<WordList>(session), "English", user.Id);
                    session.Store(wordList);
                    wordListId = wordList.Id;
                });

            // Act
            var request = new
                {
                    RequestUsername = "e@d.com",
                    RequestPassword = "pwd",
                    CreatedDate = new DateTime(2012, 1, 1),
                    Text = "t1",
                    Definition = "d1",
                    WordListName = "English",
                    OwnerEmail = "e@d.com"
                };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migrateword", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Transact(session =>
                {
                    var query = from word in session.Query<Word, WordIndex>()
                                where word.WordListId == wordListId
                                select word;
                    var newWord = query.SingleOrDefault();
                    Assert.That(newWord, Is.Not.Null);
                    Debug.Assert(newWord != null, "newWord != null");
                    Assert.That(newWord.Text, Is.EqualTo("t1"));
                    Assert.That(newWord.Definition, Is.EqualTo("d1"));
                    Assert.That(newWord.CreatedDate, Is.EqualTo(new DateTime(2012, 1, 1)));
                    var wordList = session.Load<WordList>("WordLists/1");
                    Assert.That(wordList.NumberOfWords, Is.EqualTo(1));
                });
        }

        [Test]
        public async void DoesNotCreateWordTwiceInSameWordList()
        {
            // Arrange
            Transact(session =>
            {
                var user = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username", UserRole.Admin);
                session.Store(user);
                var wordList = new WordList(KeyGeneratorBase.Generate<WordList>(session), "English", user.Id);
                session.Store(wordList);
                var generator = new KeyGenerator<Word>(session);
                var word = new Word(generator.Generate(), "t1", "d1", wordList.Id);
                session.Store(word);
            });

            // Act
            var request = new
            {
                RequestUsername = "e@d.com",
                RequestPassword = "pwd",
                CreatedDate = new DateTime(2012, 1, 1),
                Text = "t1",
                Definition = "d1",
                WordListName = "English",
                OwnerEmail = "e@d.com"
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migrateword", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Transact(session =>
                {
                    var numberOfWords = session.Query<Word, WordIndex>().Count();
                    Assert.That(numberOfWords, Is.EqualTo(1));
                });
        }

        [Test]
        public async void CreatesWordForAnotherUser()
        {
            // Arrange
            Transact(session =>
            {
                var user1 = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username", UserRole.Admin);
                session.Store(user1);
                var wordList1 = new WordList(KeyGeneratorBase.Generate<WordList>(session), "English", user1.Id);
                session.Store(wordList1);
                var generator = new KeyGenerator<Word>(session);
                var word = new Word(generator.Generate(), "t1", "d1", wordList1.Id);
                session.Store(word);

                var user2 = new User(KeyGeneratorBase.Generate<User>(session), "someone@d.com", "theirpwd", "username2");
                session.Store(user2);
                var wordList2 = new WordList(KeyGeneratorBase.Generate<WordList>(session), "English", user2.Id);
                session.Store(wordList2);
            });

            // Act
            var request = new
            {
                RequestUsername = "e@d.com",
                RequestPassword = "pwd",
                CreatedDate = new DateTime(2012, 1, 1),
                Text = "t1",
                Definition = "d1",
                WordListName = "English",
                OwnerEmail = "someone@d.com"
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migrateword", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Transact(session =>
            {
                var numberOfWords = session.Query<Word, WordIndex>().ToArray();
                Assert.That(numberOfWords, Has.Length.EqualTo(2));
            });
        }
    }
}