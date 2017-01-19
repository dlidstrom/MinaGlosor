using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.AdminCommands;
using Raven.Client;

namespace MinaGlosor.Web.Models.Commands.Handlers
{
    public class RunAdminCommandHandler : ICommandHandler<RunAdminCommand, object>
    {
        public IDocumentSession Session { get; set; }

        public IKernel Kernel { get; set; }

        public object Handle(RunAdminCommand command)
        {
            object handler = null;
            try
            {
                var adminCommand = command.AdminCommand;
                var handlerType = typeof(IAdminCommandHandler<>)
                    .MakeGenericType(adminCommand.GetType());
                handler = Kernel.Resolve(handlerType);
                var methodInfo = handlerType.GetMethod("Run");
                var result = methodInfo.Invoke(handler, new[] { (object)adminCommand });
                return result;
            }
            finally
            {
                Kernel.ReleaseComponent(handler);
            }
        }

        public bool CanExecute(RunAdminCommand command, User currentUser)
        {
            return true;
        }
    }
}