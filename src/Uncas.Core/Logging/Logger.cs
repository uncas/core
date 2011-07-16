namespace Uncas.Core.Logging
{
    using System;

    /// <summary>
    /// Default logger implementation.
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// Logs the specified log type and message.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="message">The message.</param>
        public void Log(
            LogType logType, 
            string message)
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
        /// Logs the specified log type, message and exception.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Log(
            LogType logType,
            string message,
            Exception exception)
        {
        }

        /// <summary>
        /// Logs the specified log type, message and additional info.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="message">The message.</param>
        /// <param name="additional">The additional.</param>
        public void Log(
            LogType logType,
            string message,
            string additional)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs the specified log type, message, exception, and additional info.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="additional">The additional.</param>
        public void Log(
            LogType logType,
            string message,
            Exception exception,
            string additional)
        {
            throw new NotImplementedException();
        }
    }
}