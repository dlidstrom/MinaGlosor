using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.AdminCommands;
using MinaGlosor.Web.Models.Commands;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Migrate_AdminUser : WebApiIntegrationTest
    {
        [Test]
        public async void NeedsCorrectCredentials()
        {
            // Act
            var command = new MigrateAdminUserAdminCommand("e@d.com", "pwd2");
            var request = new AdminRequest(command);
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migrateadminuser", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async void MigratesAdminUserIntoWebsiteConfig()
        {
            // Act
            var command = new MigrateAdminUserAdminCommand("e@d.com", "pwd");
            var request = new AdminRequest(command);
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migrateadminuser", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var expectedContent = new
            {
                migratedUsers = new[] { "users/1" }
            };
            Assert.That(content, Is.EqualTo(JsonConvert.SerializeObject(expectedContent)));
            Transact(session =>
            {
                var config = session.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
                Assert.That(config, Is.Not.Null);
                Assert.That(config.IsAdminUser("users/1"), Is.True);
            });
        }

        [Test]
        public async void DoesNothingIfAlreadyAdded()
        {
            // Arrange
            Transact(session =>
            {
                var config = new WebsiteConfig();
                config.AddAdminUser("users/1");
                session.Store(config);
            });

            // Act
            var command = new MigrateAdminUserAdminCommand("e@d.com", "pwd");
            var request = new AdminRequest(command);
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migrateadminuser", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        protected override void Arrange()
        {
            // Arrange
            Transact(session =>
            {
                var id = KeyGeneratorBase.Generate<User>(session);
                var user = new User(id, "e@d.com", "pwd", "admin", UserRole.Admin);
                session.Store(user);
            });
        }
    }
}