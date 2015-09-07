using System;
using System.Web.Mvc;
using MinaGlosor.Web.Infrastructure;
using MinaGlosor.Web.Infrastructure.Tracing;
using Newtonsoft.Json;
using Raven.Client;

namespace MinaGlosor.Web.Models.AdminCommands
{
    public abstract class AbstractAdminCommandHandler<TAdminCommand>
        : IAdminCommandHandler<TAdminCommand> where TAdminCommand : IAdminCommand
    {
        public IDocumentStore DocumentStore { get; set; }

        public IDocumentSession DocumentSession { get; set; }

        public abstract object Run(TAdminCommand command);

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return query.Execute(DocumentSession);
        }

        protected TResult ExecuteCommand<TResult>(ICommand<TResult> command)
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
                var commandExecutor = DependencyResolver.Current.GetService<CommandExecutor>();
                var result = (TResult)commandExecutor.ExecuteCommand(null, command);
                return result;
            }
        }
    }
}