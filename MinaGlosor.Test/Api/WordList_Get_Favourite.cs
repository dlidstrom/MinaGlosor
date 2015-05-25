using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordList_Get_Favourite : WebApiIntegrationTest
    {
        [Test]
        public async void GetsWithFavourite()
        {
            // Act
            WaitForIndexing();
            var response = await Client.GetAsync("http://temp.uri/api/wordlist");
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
                                    ownerId = "1",
                                    name = "list",
                                    numberOfWords = 10,
                                    percentDone = 0,
                                    percentExpired = 0
                                }
                        },
                    numberOfFavourites = 1
                };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        protected override async void Act()
        {
            // Arrange
            Transact(session =>
                {
                    var owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                    var anotherUser = new User(KeyGeneratorBase.Generate<User>(session), "f@d.com", "pwd", "username");
                    session.Store(owner);
                    session.Store(anotherUser);
                    var wordList = new WordList(KeyGeneratorBase.Generate<WordList>(session), "list", owner.Id);
                    session.Store(wordList);

                    // add some words to the word list
                    var generator = new KeyGenerator<Word>(session);
                    var firstWord = new Word(generator.Generate(), 1 + 1 + "t", 1 + 1 + "d", wordList.Id);
                    session.Store(firstWord);
                    for (var i = 1; i < 10; i++)
                    {
                        session.Store(new Word(generator.Generate(), 1 + i + "t", 1 + i + "d", wordList.Id));
                    }

                    // store favourite for another user
                    session.Store(new WordFavourite(firstWord.Id, anotherUser.Id));
                });

            // mark first word favourite
            var vm = new
            {
                wordId = 1
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/wordfavourite", vm);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}