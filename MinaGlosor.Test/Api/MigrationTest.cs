using System.Threading;
using Castle.Windsor;
using MinaGlosor.Test.Api.Infrastructure;

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