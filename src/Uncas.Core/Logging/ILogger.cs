namespace Uncas.Core.Logging
{
    using System;

    /// <summary>
    /// Logger interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the specified log type and description.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="description">The description.</param>
        void Log(
            LogType logType,
            string description);

        /// <summary>
        /// Logs the specified log type and exception.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="exception">The exception.</param>
        void Log(
            LogType logType,
            Exception exception);

        /// <summary>
        /// Logs the specified log type, description and exception.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="description">The description.</param>
        /// <param name="exception">The exception.</param>
        void Log(
            LogType logType,
            string description,
            Exception exception);

        /// <summary>
        /// Logs the specified log type, description and additional info.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="description">The description.</param>
        /// <param name="additional">The additional.</param>
        void Log(
            LogType logType,
            string description,
            string additional);

        /// <summary>
        /// Logs the specified log type, description, exception, and additional info.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="description">The description.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="additional">The additional.</param>
        void Log(
            LogType logType,
            string description,
            Exception exception,
            string additional);
    }
}