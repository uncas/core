namespace Uncas.Core.Logging
{
    /// <summary>
    /// The log types.
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// Indicates info for debug purposes.
        /// </summary>
        Debug = 0,

        /// <summary>
        /// Indicates info about something.
        /// </summary>
        Info = 1,

        /// <summary>
        /// Indicates a warning that something might be wrong.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Indicates an error event that needs handling.
        /// </summary>
        Error = 3,

        /// <summary>
        /// Indicates a fatal event, that typically causes termination of operation.
        /// </summary>
        Fatal = 4
    }
}