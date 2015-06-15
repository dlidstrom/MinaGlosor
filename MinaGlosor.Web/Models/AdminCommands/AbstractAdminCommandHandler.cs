using System;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using Newtonsoft.Json;
using Raven.Client;

namespace MinaGlosor.Web.Models.AdminCommands
{
    public abstract class AbstractAdminCommandHandler<TCommand> : IAdminCommandHandler<TCommand> where TCommand : IAdminCommand
    {
        public IDocumentSession DocumentSession { get; set; }

        public abstract object Run(TCommand command);

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.Execute(DocumentSession);
        }

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");

            DoExecuteCommand(command, session =>
            {
                command.Execute(session);
                return false;
            });
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            if (command == null) throw new ArgumentNullException("command");

            return DoExecuteCommand(command, command.Execute);
        }

        private TResult DoExecuteCommand<TResult, TCommandType>(
            TCommandType command,
            Func<IDocumentSession, TResult> func)
        {
            if (command == null) throw new ArgumentNullException("command");
            using (new ModelContext(ModelContext.CorrelationId))
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new PrivateMembersContractResolver(),
                    TypeNameHandling = TypeNameHandling.All
                };
                var commandAsJson = JsonConvert.SerializeObject(command, Formatting.Indented, settings);
                TracingLogger.Information(
                    EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteAdminCommand_3006,
                    commandAsJson);
                return func.Invoke(DocumentSession);
            }
        }
    }
}