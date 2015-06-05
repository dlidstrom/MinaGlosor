using System.Net.Http;
using System.Threading.Tasks;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Search_Get : WebApiIntegrationTest
    {
        [Test]
        public async void GetsMatchingWords()
        {
            // Act
            var response = await Client.GetAsync("http://temp.uri/api/search2?q=word");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.That(response.Content, Is.Not.Null);
            var searchResult = await response.Content.ReadAsAsync<SearchResult>();
            Assert.That(searchResult.Words, Has.Length.EqualTo(1));
            Assert.That(searchResult.Words[0].Text, Is.EqualTo("some word"));
            Assert.That(searchResult.Words[0].Definition, Is.EqualTo("some definition"));
        }

        [Test]
        public async void GetsSimilarWords()
        {
            // Act
            var response = await Client.GetAsync("http://temp.uri/api/search2?q=björn");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.That(response.Content, Is.Not.Null);
            var searchResult = await response.Content.ReadAsAsync<SearchResult>();
            Assert.That(searchResult.Words, Has.Length.EqualTo(1));
            Assert.That(searchResult.Words[0].Text, Is.EqualTo("isbjörn"));
            Assert.That(searchResult.Words[0].Definition, Is.EqualTo("khers ghotbi"));
        }

        protected override async void Act()
        {
            // Arrange
            Transact(session =>
            {
                var user = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                session.Store(user);
            });

            var wordListId = await PostWordList();
            PostWord("some word", "some definition", wordListId);
            PostWord("isbjörn", "khers ghotbi", wordListId);
        }

        protected async void PostWord(string text, string definition, string wordListId)
        {
            var request = new
            {
                wordListId,
                text,
                definition
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/word", request);
            Assert.That(response.IsSuccessStatusCode, Is.True);
        }

        private async Task<string> PostWordList()
        {
            var request = new
            {
                name = "Some name"
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/wordlist", request);
            Assert.That(response.IsSuccessStatusCode, Is.True);
            var content = await response.Content.ReadAsAsync<PostWordListResponse>();
            return content.WordListId;
        }

        public class SearchResult
        {
            public WordResult[] Words { get; set; }

            public class WordResult
            {
                public string Id { get; set; }

                public string Text { get; set; }

                public string Definition { get; set; }
            }
        }

        private class PostWordListResponse
        {
            public string WordListId { get; set; }
        }
    }
}