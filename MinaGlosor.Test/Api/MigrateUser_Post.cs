using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using MinaGlosor.Web.Models.Indexes;
using NUnit.Framework;
using Raven.Client.Linq;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class MigrateUser_Post : MigrationTest
    {
        [Test]
        public async void CreatesTheUser()
        {
            // Arrange
            Transact(session => session.Store(new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username", UserRole.Admin)));

            // Act
            var request = new
                {
                    RequestUsername = "e@d.com",
                    RequestPassword = "pwd",
                    Email = "someone@d.com",
                    HashedPassword = "$2a$12$mACnM5lzNigHMaf7O1py1O3vlf6.BA8k8x3IoJ.Tq3IB/2e7g61Km",
                    Username = "username2",
                    CreatedDate = new DateTime(2012, 1, 1)
                };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migrateuser", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Transact(session =>
                {
                    var query = from user in session.Query<User, UserIndex>()
                                where user.Email == "someone@d.com"
                                select user;
                    var newUser = query.SingleOrDefault();
                    Assert.That(newUser, Is.Not.Null, "New user not found");
                    Debug.Assert(newUser != null, "newUser != null");
                    Assert.That(newUser.ValidatePassword("correctbatteryhorsestapler"), Is.True, "Password not valid");
                });
        }

        [Test]
        public async void DoesNotCreateTheUserAgain()
        {
            // Arrange
            Transact(session =>
                {
                    var generator = new KeyGenerator<User>(session);
                    session.Store(new User(generator.Generate(), "e@d.com", "pwd", "username", UserRole.Admin));
                    session.Store(new User(generator.Generate(), "someone@d.com", "correctbatteryhorsesstapler", "username2"));
                });

            // Act
            var request = new
            {
                RequestUsername = "e@d.com",
                RequestPassword = "pwd",
                Email = "someone@d.com",
                HashedPassword = "$2a$12$mACnM5lzNigHMaf7O1py1O3vlf6.BA8k8x3IoJ.Tq3IB/2e7g61Km",
                Username = "username2",
                CreatedDate = new DateTime(2012, 1, 1)
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/migrateuser", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Transact(session =>
            {
                var query = from user in session.Query<User, UserIndex>()
                            select user;
                var numberOfUsers = query.Count();
                Assert.That(numberOfUsers, Is.EqualTo(2));
            });
        }
    }
}