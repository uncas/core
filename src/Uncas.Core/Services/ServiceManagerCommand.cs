namespace Uncas.Core.Services
{
    /// <summary>
    /// Commands that can be executed for a service.
    /// </summary>
    public enum ServiceManagerCommand
    {
        /// <summary>
        /// Unknown command.
        /// </summary>
        Unknown,

        /// <summary>
        /// Run as console application.
        /// </summary>
        /// <remarks>Not really a service command, 
        /// but we let the client use it as a marker
        /// for not being in service mode.</remarks>
        Application,

        /// <summary>
        /// Install as service.
        /// </summary>
        Install,

        /// <summary>
        /// Uninstall as service.
        /// </summary>
        Uninstall,

        /// <summary>
        /// Start service.
        /// </summary>
        Start,

        /// <summary>
        /// Stop service.
        /// </summary>
        Stop
    }
}