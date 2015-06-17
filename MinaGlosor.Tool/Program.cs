using System;
using System.Linq;
using Castle.Windsor;
using MinaGlosor.Tool.Commands;
using MinaGlosor.Tool.Infrastructure;
using NDesk.Options;

namespace MinaGlosor.Tool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                string username = null;
                string password = null;
                var showHelp = false;
                var optionSet = new OptionSet
                {
                    {
                        "u=|username=",
                        "Sets the logon username",
                        o => username = o
                    },
                    {
                        "p=|password=",
                        "Sets the logon password",
                        o => password = o
                    },
                    {
                        "h|help|?",
                        "Show this help",
                        o => showHelp = true
                    }
                };

                var container = ContainerBuilder.BuildContainer();
                var commandArgs = optionSet.Parse(args);

                do
                {
                    if (showHelp)
                    {
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(username))
                    {
                        Console.WriteLine("Missing username");
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(password))
                    {
                        Console.WriteLine("Missing password");
                        break;
                    }

                    if (commandArgs.Count == 0)
                    {
                        Console.WriteLine("Missing command name");
                        break;
                    }

                    var commandName = commandArgs.First();
                    var commandRunnerType = Type.GetType(string.Format("MinaGlosor.Tool.Commands.{0}CommandRunner", commandName));
                    if (commandRunnerType == null)
                    {
                        Console.WriteLine("Unrecognized command name: '{0}'", commandName);
                        break;
                    }

                    object commandRunner = null;
                    try
                    {
                        commandRunner = container.Resolve(commandName, commandRunnerType);
                        var runMethod = commandRunner.GetType().GetMethod("Run");
                        runMethod.Invoke(commandRunner, new[] { username, password, (object)commandArgs.Skip(1).ToArray() });
                    }
                    finally
                    {
                        if (commandRunner != null) container.Release(commandRunner);
                    }

                    return;
#pragma warning disable 162
                } while (false);
#pragma warning restore 162

                ShowHelp(optionSet, container);
            }
            catch (OptionException)
            {
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void ShowHelp(OptionSet optionSet, IWindsorContainer container)
        {
            Console.WriteLine("Usage: {0} <options> command", AppDomain.CurrentDomain.FriendlyName);
            Console.WriteLine("Options:");
            optionSet.WriteOptionDescriptions(Console.Out);
            Console.WriteLine();
            Console.WriteLine("Available commands:");
            var commandHandlers = container.Kernel.GetAssignableHandlers(typeof(ICommandRunner));
            var commandNames = commandHandlers.Select(x => x.ComponentModel.ComponentName);
            Console.WriteLine(string.Join(Environment.NewLine, commandNames));
        }
    }
}