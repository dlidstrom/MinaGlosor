using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
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
            Transact(session =>
                {
                    var owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                    var anotherUser = new User(KeyGeneratorBase.Generate<User>(session), "f@d.com", "pwd", "username");
                    session.Store(owner);
                    session.Store(anotherUser);
                    var wordList = new WordList(KeyGeneratorBase.Generate<WordList>(session), "list name", owner.Id);
                    session.Store(wordList);

                    var generator = new KeyGenerator<Word>(session);
                    var word = Word.Create(generator.Generate(), "w2", "d2", wordList);
                    session.Store(word);
                    session.Store(Word.Create(generator.Generate(), "w1", "d1", wordList));

                    // make one word favourite
                    session.Store(new WordFavourite(word.Id, owner.Id));

                    // store another favourite, for someone else
                    session.Store(new WordFavourite(word.Id, anotherUser.Id));
                });
        }
    }
}