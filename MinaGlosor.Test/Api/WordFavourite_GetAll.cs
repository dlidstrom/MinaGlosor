using MinaGlosor.Web.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordFavourite_GetAll : WebApiIntegrationTest
    {
        [Test]
        public async void GetsWordsForFavourites()
        {
            // Act
            var response = await Client.GetAsync("http://temp.uri/api/wordfavourite");
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = await content.ReadAsStringAsync();
            var expected = new
                {
                    wordListName = "",
                    words = new[]
                        {
                            new
                                {
                                    id = "1",
                                    text = "w2",
                                    definition = "d2"
                                }
                        }
                };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        protected override void Arrange()
        {
            // Arrange
            var owner = new User("e@d.com", "pwd", "username");
            Transact(session =>
                {
                    session.Store(owner);
                    var wordList = new WordList("list name", owner);
                    session.Store(wordList);

                    var word = new Word("w2", "d2", wordList.Id);
                    session.Store(word);
                    session.Store(new Word("w1", "d1", wordList.Id));

                    // make one word favourite
                    session.Store(new WordFavourite(word.Id, owner.Id));
                });
        }
    }
}