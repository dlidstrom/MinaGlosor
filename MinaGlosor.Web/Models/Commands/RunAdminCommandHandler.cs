using Castle.MicroKernel;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Models.AdminCommands;

namespace MinaGlosor.Web.Models.Commands
{
    public class RunAdminCommandHandler : CommandHandlerBase<RunAdminCommand, object>
    {
        private readonly IKernel kernel;

        public RunAdminCommandHandler(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override object Handle(RunAdminCommand command)
        {
            object handler = null;
            try
            {
                var handlerType = typeof(IAdminCommandHandler<>)
                    .MakeGenericType(command.AdminCommand.GetType());
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

        public override bool CanExecute(RunAdminCommand command, User currentUser)
        {
            return true;
        }
    }
}