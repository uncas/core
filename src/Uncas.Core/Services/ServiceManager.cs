namespace Uncas.Core.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration.Install;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.ServiceProcess;
    using System.Threading;

    /// <summary>
    /// Install, Uninstall, Start and Stop services.
    /// </summary>
    public sealed class ServiceManager
    {
        private Dictionary<ServiceManagerCommand, Action> _commands;

        /// <summary>
        /// Initialize a ServiceManager.
        /// </summary>
        /// <param name="serviceName">
        /// The name of the service as defined in the Service component.
        /// </param>
        public ServiceManager(string serviceName)
        {
            ServiceName = serviceName;
            InitializeCommands();
        }

        private string ServiceName { get; set; }

        /// <summary>
        /// Runs the command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void RunCommand(ServiceManagerCommand command)
        {
            if (_commands.ContainsKey(command))
            {
                _commands[command]();
            }
        }

        [SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "Is disposed in other methods in this class.")]
        private static AssemblyInstaller GetAssemblyInstaller(string[] commandLine)
        {
            var installer = new AssemblyInstaller
                                {
                                    Path = Environment.GetCommandLineArgs()[0],
                                    CommandLine = commandLine,
                                    UseNewContext = true
                                };
            return installer;
        }

        /// <summary>
        /// After a service has been installed, uninstalled, started or stopped,
        /// it might take some time
        /// for the action to complete. Wait here until we get the new status or time out.
        /// </summary>
        /// <param name="serviceController"></param>
        /// <param name="newStatus"></param>
        private static void WaitForStatusChange(
            ServiceController serviceController,
            ServiceControllerStatus newStatus)
        {
            int count = 0;
            while (serviceController.Status != newStatus && count < 30)
            {
                Thread.Sleep(1000);
                serviceController.Refresh();
                count++;
            }

            if (serviceController.Status != newStatus)
            {
                throw new InvalidOperationException("Failed to change status of service. New status: " + newStatus);
            }
        }

        private void InitializeCommands()
        {
            _commands = new Dictionary<ServiceManagerCommand, Action>
                            {
                                { ServiceManagerCommand.Install, InstallService },
                                { ServiceManagerCommand.Uninstall, UninstallService },
                                { ServiceManagerCommand.Start, StartService },
                                { ServiceManagerCommand.Stop, StopService },
                            };
        }

        /// <summary>
        /// Installs the service.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Needs to be robust - general exceptions are logged.")]
        private void InstallService()
        {
            if (IsServiceInstalled())
            {
                Console.WriteLine(
                    "Service '{0}' is already installed.", 
                    ServiceName);
                return;
            }

            try
            {
                var commandLine = new string[1];
                commandLine[0] = "Test install";
                IDictionary mySavedState = new Hashtable();
                AssemblyInstaller installer = GetAssemblyInstaller(commandLine);
                try
                {
                    installer.Install(mySavedState);
                    installer.Commit(mySavedState);
                }
                catch (Exception ex)
                {
                    installer.Rollback(mySavedState);
                    EventLog.WriteEntry(
                        "ServiceManager",
                        ex.ToString(),
                        EventLogEntryType.Error);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("ServiceManager", ex.ToString());
            }
        }

        /// <summary>
        /// Determines whether the service is installed.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the service is installed; otherwise, <c>false</c>.
        /// </returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Needs to be robust - general exceptions are logged.")]
        private bool IsServiceInstalled()
        {
            using (var serviceController =
                new ServiceController(ServiceName))
            {
                try
                {
                    ServiceControllerStatus status = serviceController.Status;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(
                        "ServiceManager",
                        ex.ToString(),
                        EventLogEntryType.Error);
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        private void StartService()
        {
            if (!IsServiceInstalled())
            {
                Console.WriteLine(
                    "Service '{0}' is not installed and will not be started.",
                    ServiceName);
                return;
            }

            using (var serviceController = new ServiceController(ServiceName))
            {
                if (serviceController.Status == ServiceControllerStatus.Stopped)
                {
                    try
                    {
                        serviceController.Start();
                        WaitForStatusChange(
                            serviceController,
                            ServiceControllerStatus.Running);
                    }
                    catch (InvalidOperationException ex)
                    {
                        EventLog.WriteEntry(
                            "ServiceManager",
                            ex.ToString(),
                            EventLogEntryType.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        private void StopService()
        {
            if (!IsServiceInstalled())
            {
                Console.WriteLine(
                    "Service '{0}' is not installed and will not be stopped.",
                    ServiceName);
                return;
            }

            using (var serviceController = new ServiceController(ServiceName))
            {
                if (serviceController.Status != ServiceControllerStatus.Running)
                {
                    return;
                }
                serviceController.Stop();
                WaitForStatusChange(serviceController, ServiceControllerStatus.Stopped);
            }
        }

        /// <summary>
        /// Uninstalls the service.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Needs to be robust - general exceptions are logged.")]
        private void UninstallService()
        {
            if (!IsServiceInstalled())
            {
                Console.WriteLine(
                    "Service '{0}' is not installed and will not be uninstalled.",
                    ServiceName);
                return;
            }

            var commandLine = new string[1];
            commandLine[0] = "Test Uninstall";
            IDictionary mySavedState = new Hashtable();
            mySavedState.Clear();
            AssemblyInstaller installer = GetAssemblyInstaller(commandLine);
            try
            {
                installer.Uninstall(mySavedState);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(
                    "ServiceManager",
                    ex.ToString(),
                    EventLogEntryType.Error);
            }
        }
    }
}