using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.BackgroundTasks;
using MinaGlosor.Web.Infrastructure.IoC;
using MinaGlosor.Web.Infrastructure.Tracing;
using MinaGlosor.Web.Models.DomainEvents;

namespace MinaGlosor.Web
{
    public class Application : HttpApplication
    {
        private static IWindsorContainer Container { get; set; }

        public static void Bootstrap(IWindsorContainer container, HttpConfiguration configuration)
        {
            WebApiConfig.Register(configuration);
            Configure(container, configuration);
        }

        public static void Shutdown()
        {
            var taskRunner = Container.Resolve<TaskRunner>();
            taskRunner.Stop(true);
            Cleanup();
        }

        public static string GetAppVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return version.ToString();
        }

        protected void Application_Start()
        {
            TracingLogger.Initialize(Server.MapPath("~/App_Data"));
            TracingLogger.Information(EventIds.Informational_Preliminary_1XXX.Web_Starting_1000, "Starting application");
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // TODO Read settings into typed class or interface
            var container = ContainerFactory.Create(int.Parse(ConfigurationManager.AppSettings["TaskRunnerPollingIntervalMillis"]));
            Configure(container, GlobalConfiguration.Configuration);
            TracingLogger.Information(EventIds.Informational_Completion_2XXX.Web_Started_2000, "Application started");
        }

        protected void Application_End()
        {
            TracingLogger.Information(
                EventIds.Information_Finalization_8XXX.Web_Stopping_8000,
                "Stopping application due to {0}",
                HostingEnvironment.ShutdownReason);
            Cleanup();
            TracingLogger.Information(EventIds.Information_Finalization_8XXX.Web_Stopped_8001, "Stopped application");
        }

        protected void Application_BeginRequest()
        {
            if (Context.IsDebuggingEnabled)
            {
                return;
            }

            if (Context.Request.IsSecureConnection == false
                && Context.Request.Url.ToString().Contains("localhost:") == false)
            {
                Response.Clear();
                Response.Status = "301 Moved Permanently";
                Response.AddHeader("Location", Context.Request.Url.ToString().Insert(4, "s"));
                Response.End();
            }
        }

        private static void Cleanup()
        {
            RouteTable.Routes.Clear();
            Container.Dispose();
        }

        private static void Configure(IWindsorContainer container, HttpConfiguration configuration)
        {
            ModelBinders.Binders[typeof(Guid)] = new GuidBinder();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DependencyResolver.SetResolver(new WindsorDependencyResolver(container));
            configuration.DependencyResolver = new WindsorHttpDependencyResolver(container.Kernel);
            DomainEvent.SetContainer(container);
            Container = container;
        }
    }
}