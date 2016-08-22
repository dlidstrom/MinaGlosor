using System.Security.Principal;
using System.Threading;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Browse_Get : WebApiIntegrationTest
    {
        [Test]
        public async void GetsAllWordLists()
        {
            // Arrange
            Transact(session =>
            {
                // fortsätt här
                session.Store(new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username", UserRole.Admin));
                session.Store(new User(KeyGeneratorBase.Generate<User>(session), "f@d.com", "pwd", "username", UserRole.Admin));
            });

            // belongs to e@d.com
            await this.PostWordList("list1");
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("f@d.com"), new string[0]);

            // belongs to f@d.com
            await this.PostWordList("list2");

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/browse");
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = await content.ReadAsStringAsync();
            var expected = new
            {
                wordLists = new[]
                {
                    new
                    {
                        wordListId = "1",
                        name = "list1"
                    },
                    new
                    {
                        wordListId = "5",
                        name = "list2"
                    }
                }
            };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }
    }
}