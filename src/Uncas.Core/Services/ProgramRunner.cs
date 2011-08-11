namespace Uncas.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.ServiceProcess;

    /// <summary>
    /// Runs programs.
    /// </summary>
    public class ProgramRunner
    {
        /// <summary>
        /// Mapping of console command line args to ServiceManagerCommands.
        /// </summary>
        private static readonly Dictionary<string, ServiceManagerCommand> _commands =
            new Dictionary<string, ServiceManagerCommand>
                {
                    { "-console", ServiceManagerCommand.Application },
                    { "-install", ServiceManagerCommand.Install },
                    { "-uninstall", ServiceManagerCommand.Uninstall },
                    { "-start", ServiceManagerCommand.Start },
                    { "-stop", ServiceManagerCommand.Stop }
                };

        /// <summary>
        /// Runs the program.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <param name="actionToRun">The action to run.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="getServiceToRun">The get service to run.</param>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Should be robust.")]
        public void RunProgram(
            string[] args,
            Action actionToRun,
            string serviceName,
            Func<ServiceBase> getServiceToRun)
        {
            if (actionToRun == null)
            {
                throw new ArgumentNullException("actionToRun");
            }

            if (string.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentNullException("serviceName");
            }

            if (getServiceToRun == null)
            {
                throw new ArgumentNullException("getServiceToRun");
            }

            try
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                if (args == null || args.Length == 0)
                {
                    ServiceBase.Run(getServiceToRun());
                    return;
                }

                ServiceManagerCommand command;
                if (!TryParseCommandLine(args, out command))
                {
                    PrintUsage();
                    return;
                }

                var serviceManager = new ServiceManager(serviceName);
                if (command == ServiceManagerCommand.Application)
                {
                    const string message = @"Running in console mode.";
                    Console.WriteLine(message);
                    actionToRun();
                    Console.Read();
                }
                else
                {
                    serviceManager.RunCommand(command);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(
                    serviceName,
                    ex.ToString(),
                    EventLogEntryType.Error);
                Console.WriteLine(ex);
            }
        }

        private static void PrintUsage()
        {
            string exeName = Assembly.GetExecutingAssembly().ManifestModule.Name;
            Console.WriteLine("Usage:");
            foreach (var item in _commands)
            {
                Console.WriteLine(@"  " + exeName + @" " + item.Key);
            }

            Console.Read();
        }

        private static bool TryParseCommandLine(
            string[] args,
            out ServiceManagerCommand command)
        {
            command = ServiceManagerCommand.Unknown;
            if (args.Length > 1)
            {
                return false;
            }

            string commandLineArg = args[0];
            if (_commands.ContainsKey(commandLineArg))
            {
                command = _commands[commandLineArg];
                return true;
            }

            return false;
        }
    }
}