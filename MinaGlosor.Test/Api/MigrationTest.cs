using System.Threading;
using Castle.Windsor;

namespace MinaGlosor.Test.Api
{
    public abstract class MigrationTest : WebApiIntegrationTest
    {
        protected override void OnSetUp(IWindsorContainer container)
        {
            Thread.CurrentPrincipal = null;
        }
    }
}