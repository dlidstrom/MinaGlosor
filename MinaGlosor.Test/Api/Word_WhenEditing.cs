using System;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Word_WhenEditing : WebApiIntegrationTest
    {
        [Test]
        public async void ResetsWordScores()
        {
            // Arrange
            Transact(session => session.Store(new User(KeyGeneratorBase.Generate<User>(session), "e@d.com", "pwd", "username")));
            var wordListResponse = await this.PostWordList();
            var wordResponse = await this.PostWord("text", "def", wordListResponse.WordListId);
            var practiceSessionResponse = await this.StartPracticeSession(wordListResponse.WordListId);

            // get next practice word
            var practiceWordResponse = await this.GetNextPracticeWord(practiceSessionResponse.PracticeSessionId);

            // post word confidence
            await this.PostWordConfidence(practiceSessionResponse.PracticeSessionId, practiceWordResponse.PracticeWordId, ConfidenceLevel.PerfectResponse);

            // Act
            await this.UpdateWord(wordResponse.WordId, "text2", "def2");

            // Assert
            // verify that word score is scheduled for repeat today
            Transact(session =>
            {
                var wordScore = session.Load<WordScore>("WordScores/1");
                Assert.That(wordScore.RepeatAfterDate, Is.LessThanOrEqualTo(DateTime.Now));
            });
        }
    }
}