using System;
using System.Linq;
using Castle.MicroKernel;
using Castle.Windsor;
using MinaGlosor.Tool.Commands;
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
                    ICommand command = null;
                    try
                    {
                        command = container.Resolve<ICommand>(commandName);
                        command.Run(username, password, commandArgs.Skip(1).ToArray());
                    }
                    catch (ComponentNotFoundException)
                    {
                        Console.WriteLine("Unrecognized command: '{0}'", commandName);
                        break;
                    }
                    finally
                    {
                        if (command != null) container.Release(command);
                    }

                    return;
                } while (false);

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
            var commandHandlers = container.Kernel.GetAssignableHandlers(typeof(ICommand));
            var commandNames = commandHandlers.Select(x => x.ComponentModel.ComponentName);
            Console.WriteLine(string.Join(Environment.NewLine, commandNames));
        }
    }
}