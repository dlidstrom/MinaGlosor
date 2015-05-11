﻿using System.Net;
using System.Net.Http;
using MinaGlosor.Web.Models;
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
            owner = new User("e@d.com", "pwd", "username");
            Transact(session =>
            {
                session.Store(owner);
                var wordList = new WordList("list", owner);
                session.Store(wordList);
                word = new Word("some text", "some def", wordList.Id);
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