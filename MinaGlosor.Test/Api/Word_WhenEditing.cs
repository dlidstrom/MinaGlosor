using System;
using System.Threading;
using MinaGlosor.Test.Api.Infrastructure;
using MinaGlosor.Web.Infrastructure.BackgroundTasks;
using MinaGlosor.Web.Models;
using MinaGlosor.Web.Models.Commands.Handlers;
using NUnit.Framework;
using Raven.Abstractions;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class Word_WhenEditing : WebApiIntegrationTest
    {
        private AutoResetEvent ev;

        [Test]
        public async void ResetsWordScores()
        {
            // Arrange
            var taskRunner = Container.Resolve<TaskRunner>();
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

            ev = new AutoResetEvent(false);
            taskRunner.ProcessedTasks += TaskRunnerOnProcessedTasks;
            ev.WaitOne();

            // Assert
            // verify that word score is scheduled for repeat today
            Transact(session =>
            {
                var wordScore = session.Load<WordScore>("WordScores/1");
                Assert.That(wordScore.RepeatAfterDate, Is.LessThanOrEqualTo(SystemTime.UtcNow));
            });

            taskRunner.ProcessedTasks -= TaskRunnerOnProcessedTasks;
        }

        private void TaskRunnerOnProcessedTasks(object sender, EventArgs eventArgs)
        {
            ev.Set();
        }
    }
}