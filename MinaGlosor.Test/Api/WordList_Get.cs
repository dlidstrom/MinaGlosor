using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordList_Get : WebApiIntegrationTest
    {
        [Test]
        public async void GetsById()
        {
            // Arrange
            Transact(session =>
            {
                session.Store(new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username"));
            });
            await this.PostWordList();

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/wordlist?wordListId=1");
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = await content.ReadAsStringAsync();
            var expected = new
            {
                wordListId = "1",
                ownerId = "1",
                name = "some name",
                numberOfWords = 2,
                publishState = "Private"
            };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }
    }
}