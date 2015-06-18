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
    public class MigrateWordList_Post : MigrationTest
    {
        [Test]
        public async void CreatesTheWordList()
        {
            // Arrange
            string userId = null;
            Transact(session =>
                {
                    var user = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username", UserRole.Admin);
                    session.Store(user);
                    userId = user.Id;
                });

            // Act
            var request = new
                {
                    RequestUsername = "e@d.com",
                    RequestPassword = "pwd",
                    Name = "English",
                    OwnerEmail = "e@d.com"
                };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migratewordlist", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Transact(session =>
                {
                    var query = from wordList in session.Query<WordList, WordListIndex>()
                                where wordList.OwnerId == userId
                                select wordList;
                    var newWordList = query.SingleOrDefault();
                    Assert.That(newWordList, Is.Not.Null);
                    Debug.Assert(newWordList != null, "newWordList != null");
                    Assert.That(newWordList.Name, Is.EqualTo("English"));
                });
        }

        [Test]
        public async void DoesNotCreateWordListTwiceForSameUser()
        {
            // Arrange
            Transact(session =>
                {
                    var user = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username", UserRole.Admin);
                    session.Store(user);
                    session.Store(new WordList(KeyGeneratorBase.Generate<WordList>(session), "English", user.Id));
                });

            // Act
            var request = new
            {
                RequestUsername = "e@d.com",
                RequestPassword = "pwd",
                Name = "English",
                OwnerEmail = "e@d.com"
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migratewordlist", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Transact(session =>
                {
                    var query = from wordList in session.Query<WordList, WordListIndex>()
                                select wordList;
                    var numberOfWordLists = query.Count();
                    Assert.That(numberOfWordLists, Is.EqualTo(1));
                });
        }

        [Test]
        public async void CreateWordListForAnotherUser()
        {
            // Arrange
            Transact(session =>
            {
                var user = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username", UserRole.Admin);
                session.Store(user);
                session.Store(new WordList(KeyGeneratorBase.Generate<WordList>(session), "English", user.Id));
                session.Store(new User(KeyGeneratorBase.Generate<User>(session), "someone@d.com", "theirpwd", "username2"));
            });

            // Act
            var request = new
            {
                RequestUsername = "e@d.com",
                RequestPassword = "pwd",
                Name = "English",
                OwnerEmail = "someone@d.com"
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migratewordlist", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Transact(session =>
            {
                var query = from wordList in session.Query<WordList, WordListIndex>()
                            select wordList;
                var numberOfWordLists = query.Count();
                Assert.That(numberOfWordLists, Is.EqualTo(2));
            });
        }
    }
}