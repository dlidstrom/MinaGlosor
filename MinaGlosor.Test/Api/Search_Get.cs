using System;
using System.Collections.Generic;
using System.Net.Http;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Search_Get : WebApiIntegrationTest
    {
        private static IEnumerable<Result> Results
        {
            get
            {
                yield return Result.IsMatch(
                    "björn",
                    "<b style=\"background:lawngreen\">isbjörn</b>",
                    "khers ghotbi");

                yield return Result.IsMatch(
                    "hotbi",
                    "isbjörn",
                    "khers <b style=\"background:lawngreen\">ghotbi</b>");

                yield return Result.IsMatch(
                    "stark",
                    "<b style=\"background:lawngreen\">stor</b>",
                    "bozorg");

                yield return Result.IsMatch(
                    "kon!",
                    "ta'rif <b style=\"background:yellow\">kon</b>!",
                    "vad säger du? berätta!");

                yield return Result.IsMatch(
                    "berätta!",
                    "ta'rif kon!",
                    "vad säger du? <b style=\"background:yellow\">berätta</b>!");
            }
        }

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
            Assert.That(searchResult.Words[0].Text, Is.EqualTo("some <b style=\"background:yellow\">word</b>"));
            Assert.That(searchResult.Words[0].Definition, Is.EqualTo("some definition"));
        }

        [TestCaseSource("Results")]
        public async void GetsSimilarWords(Result result)
        {
            // Act
            var response = await Client.GetAsync(string.Format("http://temp.uri/api/search2?q={0}", result.Q));
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.That(response.Content, Is.Not.Null);
            var searchResult = await response.Content.ReadAsAsync<SearchResult>();
            result.Verify(searchResult);
        }

        protected override async void Act()
        {
            // Arrange
            Transact(session =>
            {
                var user = new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username");
                session.Store(user);
            });

            var wordListResponse = await this.PostWordList();
            await this.PostWord("some word", "some definition", wordListResponse.WordListId);
            await this.PostWord("isbjörn", "khers ghotbi", wordListResponse.WordListId);
            await this.PostWord("stor", "bozorg", wordListResponse.WordListId);
            await this.PostWord("ta'rif kon!", "vad säger du? berätta!", wordListResponse.WordListId);
        }

        public class Result
        {
            private Action<SearchResult> verify;

            public string Q { get; private set; }

            public string ExpectedText { get; private set; }

            public string ExpectedDefinition { get; private set; }

            public static Result IsMatch(string q, string text, string definition)
            {
                var result = new Result
                    {
                        Q = q,
                        ExpectedText = text,
                        ExpectedDefinition = definition
                    };
                result.verify = result.VerifyIsMatch;
                return result;
            }

            public override string ToString()
            {
                return string.Format("Q={0} ExpectedText={1} ExpectedDefinition={2}", Q, ExpectedText, ExpectedDefinition);
            }

            public void Verify(SearchResult searchResult)
            {
                verify.Invoke(searchResult);
            }

            private void VerifyIsMatch(SearchResult searchResult)
            {
                Assert.That(searchResult.Words, Has.Length.EqualTo(1));
                Assert.That(searchResult.Words[0].Text, Is.EqualTo(ExpectedText));
                Assert.That(searchResult.Words[0].Definition, Is.EqualTo(ExpectedDefinition));
            }

            private void VerifyIsNoMatch(SearchResult searchResult)
            {
                Assert.That(searchResult.Words, Is.Empty);
            }
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
    }
}