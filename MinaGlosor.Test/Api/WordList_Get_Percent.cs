using System.Net.Http;
using MinaGlosor.Web.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class WordList_Get_Percent : WebApiIntegrationTest
    {
        [Test]
        public async void GetsWithPercentComplete()
        {
            // Act
            WaitForIndexing();
            var response = await Client.GetAsync("http://temp.uri/api/wordlist");
            var content = response.Content;

            // Assert
            Assert.That(content, Is.Not.Null);
            var result = await content.ReadAsStringAsync();
            var expected = new[]
                {
                    new
                        {
                            wordListId = "1",
                            ownerId = "1",
                            name = "list",
                            numberOfWords = 10,
                            percentDone = 10
                        }
                };
            Assert.That(result, Is.EqualTo(JsonConvert.SerializeObject(expected)));
        }

        protected override async void Act()
        {
            // Arrange
            Transact(session =>
            {
                var owner = new User("e@d.com", "pwd", "username");
                session.Store(owner);
                var wordList = new WordList("list", owner);
                session.Store(wordList);

                // add some words to the word list
                for (var i = 0; i < 10; i++)
                {
                    session.Store(new Word(1 + i + "t", 1 + i + "d", wordList.Id));
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
            var getWordResponse = await Client.GetAsync("http://temp.uri/api/practiceword?practiceSessionId=" + createSessionContent.PracticeSessionId);
            Assert.That(getWordResponse.Content, Is.Not.Null);
            var getWordContent = await getWordResponse.Content.ReadAsAsync<GetWordContent>();

            var postWordConfidenceRequest = new
            {
                createSessionContent.PracticeSessionId,
                getWordContent.PracticeWordId,
                ConfidenceLevel = "PerfectResponse"
            };
            var wordConfidenceResponse = await Client.PostAsJsonAsync(
                "http://temp.uri/api/wordconfidence", postWordConfidenceRequest);
            Assert.That(wordConfidenceResponse.Content, Is.Not.Null);
            var wordConfidenceContent = await wordConfidenceResponse.Content.ReadAsAsync<WordConfidenceContent>();
            Assert.That(wordConfidenceContent.IsFinished, Is.False);
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