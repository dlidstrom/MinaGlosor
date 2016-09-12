using System.Net.Http;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class PublishWordList_Post : WebApiIntegrationTest
    {
        [Test]
        public async void Publish()
        {
            // Arrange
            var wordListResponse = await this.PostWordList("list");

            // Act
            var response = await Client.PostAsJsonAsync(
                "http://temp.uri/api/publishwordlist",
                new
                {
                    wordListId = wordListResponse.WordListId,
                    publish = true
                });

            // Assert
            response.EnsureSuccessStatusCode();
            Transact(session =>
            {
                var wordList = session.Load<WordList>(WordList.ToId(wordListResponse.WordListId));
                Assert.That(wordList.PublishState, Is.EqualTo(WordListPublishState.Published));
            });
        }

        [Test]
        public async void Unpublish()
        {
            // Arrange
            var wordListResponse = await this.PostWordList("list");
            Transact(session =>
            {
                var wordList = session.Load<WordList>(WordList.ToId(wordListResponse.WordListId));
                wordList.Publish();
            });

            // Act
            var response = await Client.PostAsJsonAsync(
                "http://temp.uri/api/publishwordlist",
                new
                {
                    wordListId = wordListResponse.WordListId,
                    publish = false
                });

            // Assert
            response.EnsureSuccessStatusCode();
            Transact(session =>
            {
                var wordList = session.Load<WordList>(WordList.ToId(wordListResponse.WordListId));
                Assert.That(wordList.PublishState, Is.EqualTo(WordListPublishState.Private));
            });
        }

        protected override void Arrange()
        {
            Transact(session =>
            {
                session.Store(new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username"));
            });
        }
    }
}