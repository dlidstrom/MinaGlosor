using Castle.Windsor;

namespace MinaGlosor.Tool
{
    public static class ContainerBuilder
    {
        public static IWindsorContainer BuildContainer()
        {
            var container = new WindsorContainer();
            container.Install(new CommandsInstaller());
            return container;
        }
    }
}