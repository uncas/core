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

        private readonly Action _actionToRun;
        private readonly Func<ServiceBase> _getServiceToRun;
        private readonly string _serviceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramRunner"/> class.
        /// </summary>
        /// <param name="actionToRun">The action to run.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="getServiceToRun">The get service to run.</param>
        public ProgramRunner(
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

            _actionToRun = actionToRun;
            _serviceName = serviceName;
            _getServiceToRun = getServiceToRun;
        }

        /// <summary>
        /// Runs the program.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Should be robust.")]
        public void RunProgram(string[] args)
        {
            try
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                if (args == null || args.Length == 0)
                {
                    ServiceBase.Run(_getServiceToRun());
                    return;
                }

                ServiceManagerCommand command;
                if (!TryParseCommandLine(args, out command))
                {
                    PrintUsage();
                    return;
                }

                var serviceManager = new ServiceManager(_serviceName);
                if (command == ServiceManagerCommand.Application)
                {
                    Console.WriteLine(CoreText.ProgramRunner_RunningInConsoleMode);
                    _actionToRun();
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
                    _serviceName,
                    ex.ToString(),
                    EventLogEntryType.Error);
                Console.WriteLine(ex);
            }
        }

        private static void PrintUsage()
        {
            string exeName = Assembly.GetExecutingAssembly().ManifestModule.Name;
            Console.WriteLine(CoreText.ProgramRunner_Usage);
            foreach (var item in _commands)
            {
                Console.WriteLine(
                    CoreText.ProgramRunner_CommandUsage,
                    exeName,
                    item.Key);
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