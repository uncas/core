namespace Uncas.Core.Logging
{
    using System;

    /// <summary>
    /// Default logger implementation.
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// Logs the specified log type and description.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="description">The description.</param>
        public void Log(
            LogType logType,
            string description)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs the specified log type and exception.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="exception">The exception.</param>
        public void Log(
            LogType logType,
            Exception exception)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs the specified log type, description and exception.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="description">The description.</param>
        /// <param name="exception">The exception.</param>
        public void Log(
            LogType logType,
            string description,
            Exception exception)
        {
        }

        /// <summary>
        /// Logs the specified log type, description and additional info.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="description">The description.</param>
        /// <param name="additional">The additional.</param>
        public void Log(
            LogType logType,
            string description,
            string additional)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs the specified log type, description, exception, and additional info.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="description">The description.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="additional">The additional.</param>
        public void Log(
            LogType logType,
            string description,
            Exception exception,
            string additional)
        {
            throw new NotImplementedException();
        }
    }
}