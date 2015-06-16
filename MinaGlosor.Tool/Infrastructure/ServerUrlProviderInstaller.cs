using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace MinaGlosor.Tool.Infrastructure
{
    public class ServerUrlProviderInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ServerUrlProvider>()
                         .ImplementedBy<ServerUrlProvider>()
                         .LifestyleSingleton()
                         .DependsOn(Dependency.OnAppSettingsValue("serverUrl", "ServerUrl")));
        }
    }
}