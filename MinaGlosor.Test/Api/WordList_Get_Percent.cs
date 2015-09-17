using System;
using System.Net.Http;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using Newtonsoft.Json;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordList_Get_Percent : WebApiIntegrationTest
    {
        [Test]
        public async void GetsWithPercentComplete()
        {
            // Act
            SystemTime.UtcDateTime = () => new DateTime(2012, 1, 2, 0, 0, 5);
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
                                    percentDone = 20,
                                    percentExpired = 10
                                }
                        },
                    numberOfFavourites = 0
                };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        protected override async void Act()
        {
            // Arrange
            SystemTime.UtcDateTime = () => new DateTime(2012, 1, 1);
            Transact(session =>
            {
                var owner = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                session.Store(owner);
                var wordList = new WordList(KeyGeneratorBase.Generate<WordList>(session), "list", owner.Id);
                session.Store(wordList);

                // add some words to the word list
                var generator = new KeyGenerator<Word>(session);
                for (var i = 0; i < 10; i++)
                {
                    session.Store(Word.Create(generator.Generate(), 1 + i + "t", 1 + i + "d", wordList));
                }
            });

            // practice the first word
            var request = new
            {
                wordListId = "1"
            };
            var createSessionResponse = await Client.PostAsJsonAsync("http://temp.uri/api/practicesession", request);
            Assert.That(createSessionResponse.Content, Is.Not.Null);
            var createSessionContent = await createSessionResponse.Content.ReadAsAsync<CreateSessionContent>();

            for (var i = 0; i < 2; i++)
            {
                var second = 10 * i;
                SystemTime.UtcDateTime = () => new DateTime(2012, 1, 1, 0, 0, second);
                var getWordResponse1 = await Client.GetAsync("http://temp.uri/api/practiceword?practiceSessionId=" + createSessionContent.PracticeSessionId);
                Assert.That(getWordResponse1.Content, Is.Not.Null);
                var getWordContent = await getWordResponse1.Content.ReadAsAsync<GetWordContent>();

                var postWordConfidenceRequest1 = new
                {
                    createSessionContent.PracticeSessionId,
                    getWordContent.PracticeWordId,
                    ConfidenceLevel = "PerfectResponse"
                };
                var wordConfidenceResponse = await Client.PostAsJsonAsync(
                    "http://temp.uri/api/wordconfidence", postWordConfidenceRequest1);
                Assert.That(wordConfidenceResponse.Content, Is.Not.Null);
                var wordConfidenceContent = await wordConfidenceResponse.Content.ReadAsAsync<WordConfidenceContent>();
                Assert.That(wordConfidenceContent.IsFinished, Is.False);
            }
        }

        public class CreateSessionContent
        {
            public string PracticeSessionId { get; set; }
        }

        public class WordConfidenceContent
        {
            public bool IsFinished { get; set; }
        }

        public class GetWordContent
        {
            public string PracticeWordId { get; set; }
        }
    }
}