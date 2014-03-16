using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using Castle.Windsor;
using MinaGlosor.Web.Data.Models;
using MinaGlosor.Web.Infrastructure;
using NUnit.Framework;

namespace MinaGlosor.Test.Api
{
    [TestFixture]
    public class PracticeSession_Post : WebApiIntegrationTest
    {
        private HttpResponseMessage result;

        [Test]
        public void CreatesNewPracticeSession()
        {
            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Transact(context =>
                {
                    var practiceSession = context.PracticeSessions.SingleOrDefault(x => x.WordListId == 2);
                    Assert.That(practiceSession, Is.Not.Null);
                    Debug.Assert(practiceSession != null, "practiceSession != null");
                    Assert.That(practiceSession.ValidFrom, Is.EqualTo(new DateTime(2014, 3, 16)));
                    Assert.That(practiceSession.ValidTo, Is.Null);
                    Assert.That(practiceSession.WordListId, Is.EqualTo(2));
                });
        }

        [Test]
        public void ClosesPreviousPracticeSessions()
        {
            // Assert
            Transact(context =>
                {
                    var existing = context.PracticeSessions.Single(x => x.WordListId == 1);
                    Assert.That(existing.ValidTo, Is.EqualTo(new DateTime(2014, 3, 16)));
                });
        }

        protected override void Act()
        {
            // Arrange
            var vm = new
            {
                wordListId = 2
            };
            SystemTime.UtcDateTime = () => new DateTime(2014, 3, 16);

            // Act
            var response = Client.PostAsJsonAsync("http://temp.uri/api/practicesession", vm);
            result = response.Result;
        }

        protected override void OnSetUp(IWindsorContainer container)
        {
            // Arrange
            var owner = new User("e@d.com", "pwd") { Id = 1 };
            Transact(context =>
                {
                    context.Users.Add(owner);
                    var wordList1 = new WordList("wl1", owner) { Id = 1 };
                    var wordList2 = new WordList("wl2", owner) { Id = 2 };
                    context.WordLists.Add(wordList1);
                    context.WordLists.Add(wordList2);
                    context.PracticeSessions.Add(new PracticeSession(wordList1));
                });

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("e@d.com"), new string[0]);
        }

        protected override void OnTearDown()
        {
            SystemTime.UtcDateTime = null;
        }
    }
}