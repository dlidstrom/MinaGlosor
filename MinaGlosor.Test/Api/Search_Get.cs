﻿using System;
using System.Collections.Generic;
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
        private static IEnumerable<Result> Results
        {
            get
            {
                yield return Result.IsMatch("björn", "isbjörn", "khers ghotbi");
                yield return Result.IsMatch("hotbi", "isbjörn", "khers ghotbi");
                yield return Result.IsMatch("stark", "stor", "bozorg");
                yield return Result.IsMatch("kon!", "ta'rif kon!", "vad säger du? berätta!");
                yield return Result.IsMatch("berätta!", "ta'rif kon!", "vad säger du? berätta!");
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
            Assert.That(searchResult.Words[0].Text, Is.EqualTo("some word"));
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

            var wordListId = await PostWordList();
            PostWord("some word", "some definition", wordListId);
            PostWord("isbjörn", "khers ghotbi", wordListId);
            PostWord("stor", "bozorg", wordListId);
            PostWord("ta'rif kon!", "vad säger du? berätta!", wordListId);
        }

        private async void PostWord(string text, string definition, string wordListId)
        {
            var request = new
            {
                wordListId,
                text,
                definition
            };
            var response = await Client.PostAsJsonAsync("http://temp.uri/api/word", request);
            Assert.That(response.IsSuccessStatusCode, Is.True);
            WaitForIndexing();
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
            WaitForIndexing();
            return content.WordListId;
        }

        public class Result
        {
            private Action<SearchResult> verify;

            public string Q { get; private set; }

            public string Text { get; private set; }

            public string Definition { get; private set; }

            public static Result IsMatch(string q, string text, string definition)
            {
                var result = new Result
                    {
                        Q = q,
                        Text = text,
                        Definition = definition
                    };
                result.verify = result.VerifyIsMatch;
                return result;
            }

            public static Result IsNoMatch(string q)
            {
                var result = new Result
                    {
                        Q = q
                    };
                result.verify = result.VerifyIsNoMatch;
                return result;
            }

            public override string ToString()
            {
                return string.Format("Q={0} Text={1} Definition={2}", Q, Text, Definition);
            }

            public void Verify(SearchResult searchResult)
            {
                verify.Invoke(searchResult);
            }

            private void VerifyIsMatch(SearchResult searchResult)
            {
                Assert.That(searchResult.Words, Has.Length.EqualTo(1));
                Assert.That(searchResult.Words[0].Text, Is.EqualTo(Text));
                Assert.That(searchResult.Words[0].Definition, Is.EqualTo(Definition));
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

        private class PostWordListResponse
        {
            public string WordListId { get; set; }
        }
    }
}