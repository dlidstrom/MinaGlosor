using MinaGlosor.Test.Api.Infrastructure;
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
            await this.PostWordList("name");

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
                name = "Some name",
                numberOfWords = 2,
                publishState = "Private"
            };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }
    }
}