using Castle.Windsor;

namespace MinaGlosor.Tool.Infrastructure
{
    public static class ContainerBuilder
    {
        public static IWindsorContainer BuildContainer()
        {
            var container = new WindsorContainer();
            container.Install(new CommandRunnerInstaller(), new ServerUrlProviderInstaller());
            return container;
        }
    }
}