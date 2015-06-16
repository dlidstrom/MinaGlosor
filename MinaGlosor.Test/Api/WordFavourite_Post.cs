using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordFavourite_Post : WebApiIntegrationTest
    {
        private string content;
        private Word word;
        private User owner;
        private HttpResponseMessage response;

        [Test]
        public void Succeeds()
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async void SetsFavourite()
        {
            var expected = new
                {
                    isFavourite = true
                };
            Assert.That(response.Content, Is.Not.Null);
            content = await response.Content.ReadAsStringAsync();
            Assert.That(content, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        [Test]
        public void CreatesFavourite()
        {
            Transact(session =>
                {
                    var wordFavourite = session.Load<WordFavourite>(WordFavourite.GetId(word.Id, owner.Id));
                    Assert.That(wordFavourite, Is.Not.Null);
                    Assert.That(wordFavourite.IsFavourite, Is.True);
                });
        }

        protected override async void Act()
        {
            // Arrange
            Transact(session =>
            {
                owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                session.Store(owner);
                var wordList = new WordList(KeyGeneratorBase.Generate<WordList>(session), "list", owner.Id);
                session.Store(wordList);
                var generator = new KeyGenerator<Word>(session);
                word = Word.Create(generator.Generate(), "some text", "some def", wordList);
                session.Store(word);
            });

            // Act
            var vm = new
            {
                wordId = 1
            };
            response = await Client.PostAsJsonAsync("http://temp.uri/api/wordfavourite", vm);
        }
    }
}