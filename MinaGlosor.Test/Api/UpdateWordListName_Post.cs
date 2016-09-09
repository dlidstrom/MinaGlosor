using System.Net;
using System.Net.Http;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class UpdateWordListName_Post : WebApiIntegrationTest
    {
        [Test]
        public async void ItShouldUpdateWordListName()
        {
            // Arrange
            Transact(session =>
            {
                var user = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                session.Store(user);
            });
            var firstResponse = await this.PostWordList("first name");

            // Act
            var request = new
            {
                wordListId = firstResponse.WordListId,
                wordListName = "second name"
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/updatewordlistname", request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Transact(session =>
            {
                var wordList = session.Load<WordList>(WordList.ToId(firstResponse.WordListId));
                Assert.That(wordList.Name, Is.EqualTo("second name"));
            });
        }
    }
}