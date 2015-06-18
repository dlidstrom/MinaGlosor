using System.Net.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.AdminCommands;
using MinaGlosor.Web.Models.Commands;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Migrate_WordUserId : MigrationTest
    {
        [Test]
        public async void SetsUserId()
        {
            // Arrange
            string userId = null;
            string wordId = null;
            Transact(session =>
            {
                var user = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "admin", UserRole.Admin);
                session.Store(user);
                userId = user.Id;

                var wordList = new WordList(KeyGeneratorBase.Generate<WordList>(session), "wl name", user.Id);
                session.Store(wordList);

                var word = Word.Create(KeyGeneratorBase.Generate<Word>(session), "w text", "w def", wordList);
                word.GetType().GetProperty("UserId").SetValue(word, null);
                session.Store(word);
                wordId = word.Id;
            });

            // Act
            var command = new MigrateWordUserIdAdminCommand("e@d.com", "pwd");
            var request = new AdminRequest(command);
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/admincommand", request);
            response.EnsureSuccessStatusCode();
            Assert.That(response.Content, Is.Not.Null);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            var expectedContent = new
            {
                migratedWords = new[] { "words/1" }
            };
            Assert.That(content, Is.EqualTo(JsonConvert.SerializeObject(expectedContent)));
            Transact(session =>
            {
                var word = session.Load<Word>(wordId);
                Assert.That(word.UserId, Is.EqualTo(userId));
            });
        }
    }
}