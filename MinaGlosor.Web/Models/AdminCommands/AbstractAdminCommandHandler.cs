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

        public QueryExecutor QueryExecutor { get; set; }

        public abstract object Run(TAdminCommand command);

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");

            TracingLogger.Information(
                EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteAdminQuery_3017,
                query.ToJson());

            // already in scope from RunAdminCommand
            var result = QueryExecutor.ExecuteQuery(query, null);

            TracingLogger.Information(
                EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteAdminQueryResult_3018,
                result.ToJson());
            return result;
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            if (command == null) throw new ArgumentNullException("command");

            TracingLogger.Information(
                EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteAdminCommand_3006,
                command.ToJson());

            // already in scope from RunAdminCommand
            var result = CommandExecutor.ExecuteCommand(command, null);

            TracingLogger.Information(
                EventIds.Informational_ApplicationLog_3XXX.Web_ExecuteAdminCommandResult_3016,
                result.ToJson());
            return result;
        }
    }
}