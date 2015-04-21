using MinaGlosor.Web.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordFavourite_Post : WebApiIntegrationTest
    {
        [Test]
        public async void SetsFavourite()
        {
            // Arrange
            var owner = new User("e@d.com", "pwd", "username");
            Transact(session =>
                {
                    session.Store(owner);
                    var wordList = new WordList("list", owner);
                    session.Store(wordList);
                    var word = new Word("some text", "some def", wordList.Id);
                    session.Store(word);
                });

            // Act
            var response = await Client.GetAsync("http://temp.uri/api/wordfavourite?wordId=1");
            var content = await response.Content.ReadAsStringAsync();
            var expected = new
                {
                    isFavourite = true
                };
            Assert.That(content, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }
    }
}