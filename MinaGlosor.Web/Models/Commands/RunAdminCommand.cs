using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.AdminCommands;
using Newtonsoft.Json;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands
{
    public class RunAdminCommand : ICommand<object>
    {
        [JsonIgnore]
        private readonly IKernel kernel;

        private readonly IAdminCommand command;

        public RunAdminCommand(IKernel kernel, IAdminCommand command)
        {
            this.kernel = kernel;
            this.command = command;
        }

        public bool CanExecute(IDocumentSession session, User currentUser)
        {
            return true;
        }

        public object Execute(IDocumentSession session)
        {
            object handler = null;
            try
            {
                var handlerType = typeof(IAdminCommandHandler<>).MakeGenericType(command.GetType());
                handler = kernel.Resolve(handlerType);
                var methodInfo = handlerType.GetMethod("Run");
                var result = methodInfo.Invoke(handler, new[] { (object)command });
                return result;
            }
            finally
            {
                kernel.ReleaseComponent(handler);
            }
        }
    }
}