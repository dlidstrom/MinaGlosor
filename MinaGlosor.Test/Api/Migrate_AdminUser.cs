using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Models;
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
            var vm = new
            {
                RequestUsername = "e@d.com",
                RequestPassword = "pwd2",
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migrateadminuser", vm);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async void MigratesAdminUserIntoWebsiteConfig()
        {
            // Act
            var vm = new
            {
                RequestUsername = "e@d.com",
                RequestPassword = "pwd",
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migrateadminuser", vm);

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
            var vm = new
            {
                RequestUsername = "e@d.com",
                RequestPassword = "pwd",
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migrateadminuser", vm);

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