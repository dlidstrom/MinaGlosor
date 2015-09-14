using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using MinaGlosor.Web;
using MinaGlosor.Web.Infrastructure.BackgroundTasks;
using MinaGlosor.Web.Infrastructure.IoC.Installers;
using NUnit.Framework;
using Raven.Client;

namespace MinaGlosor.Test.Api.Infrastructure
{
    public abstract class WebApiIntegrationTest : ExceptionLogger
    {
        private AutoResetEvent taskRunnerEvent;

        public HttpClient Client { get; private set; }

        protected IWindsorContainer Container { get; set; }

        public override void Log(ExceptionLoggerContext context)
        {
            Debug.WriteLine(context.Exception.ToString());
        }

        [SetUp]
        public void SetUp()
        {
            var configuration = new HttpConfiguration();
            configuration.Services.Add(typeof(IExceptionLogger), this);
            Container = new WindsorContainer();
            Container.AddFacility<StartableFacility>();
            Container.Install(
                RavenInstaller.CreateForTests(),
                new WindsorWebApiInstaller(),
                new HandlersInstaller(),
                new AdminCommandHandlerInstaller(),
                new BackgroundTaskHandlerInstaller(),
                new CommandQueryInstaller(),
                new TaskRunnerInstaller(500));
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("e@d.com"), new string[0]);
            OnSetUp(Container);

            Application.Bootstrap(Container, configuration);
            Client = new HttpClient(new HttpServer(configuration));

            Arrange();
            Act();
        }

        [TearDown]
        public void TearDown()
        {
            OnTearDown();
            Application.Shutdown();
        }

        public void WaitForIndexing()
        {
            // wait for tasks
            var taskRunner = Container.Resolve<TaskRunner>();
            taskRunner.ProcessedTasks += TaskRunnerOnProcessedTasks;
            taskRunnerEvent = new AutoResetEvent(false);
            Debug.WriteLine("Waiting for task runner");
            taskRunnerEvent.WaitOne();
            Debug.WriteLine("Task runner done");
            taskRunner.ProcessedTasks -= TaskRunnerOnProcessedTasks;

            // wait for indexes
            Debug.WriteLine("Waiting for indexing");
            var documentStore = Container.Resolve<IDocumentStore>();
            var indexingTask = Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {
                        var staleIndexes = documentStore.DatabaseCommands
                                                        .GetStatistics()
                                                        .StaleIndexes;
                        Debug.WriteLine("Stale indexes: {0}", staleIndexes.Length);
                        if (staleIndexes.Length == 0)
                        {
                            break;
                        }

                        Task.Delay(500).Wait();
                    }
                });
            indexingTask.Wait();
            Debug.WriteLine("Indexing done");
        }

        protected virtual void Arrange()
        {
        }

        protected virtual void Act()
        {
        }

        protected void Transact(Action<IDocumentSession> action)
        {
            WaitForIndexing();

            using (Container.BeginScope())
            using (var session = Container.Resolve<IDocumentSession>())
            {
                action.Invoke(session);
                session.SaveChanges();
            }

            WaitForIndexing();
        }

        protected virtual void OnSetUp(IWindsorContainer container)
        {
        }

        protected virtual void OnTearDown()
        {
        }

        private void TaskRunnerOnProcessedTasks(object sender, EventArgs eventArgs)
        {
            taskRunnerEvent.Set();
        }
    }
}