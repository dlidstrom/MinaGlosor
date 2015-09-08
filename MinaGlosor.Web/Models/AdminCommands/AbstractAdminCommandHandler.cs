using System;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using Raven.Client;

namespace MinaGlosor.Web.Models.AdminCommands
{
    public abstract class AbstractAdminCommandHandler<TAdminCommand>
        : IAdminCommandHandler<TAdminCommand> where TAdminCommand : IAdminCommand
    {
        public IDocumentStore DocumentStore { get; set; }

        public IDocumentSession DocumentSession { get; set; }

        public CommandExecutor CommandExecutor { get; set; }

        public abstract object Run(TAdminCommand command);

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.Execute(DocumentSession);
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            if (command == null) throw new ArgumentNullException("command");

            var commandAsJson = command.ToJson();
            TracingLogger.Information(
                EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteAdminCommand_3006,
                commandAsJson);
            var result = CommandExecutor.ExecuteCommand(command, null);
            return result;
        }
    }
}