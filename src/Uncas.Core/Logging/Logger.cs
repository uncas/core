namespace Uncas.Core.Logging
{
    using System;

    /// <summary>
    /// Default logger implementation.
    /// </summary>
    public class Logger : ILogger
    {
        private readonly ILogRepository _logRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="logRepository">The log repository.</param>
        public Logger(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        /// <summary>
        /// Logs the specified log type and description.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="description">The description.</param>
        public void Log(
            LogType logType,
            string description)
        {
            GenerateAndSaveLogEntry(
                logType,
                description,
                null,
                null);
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
            GenerateAndSaveLogEntry(
                logType,
                null,
                exception,
                null);

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
            GenerateAndSaveLogEntry(
                logType,
                description,
                exception,
                null);
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
            GenerateAndSaveLogEntry(
                logType,
                description,
                null,
                additional);

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
            GenerateAndSaveLogEntry(
               logType,
               description,
               exception,
               additional);
        }

        private void GenerateAndSaveLogEntry(
            LogType logType,
            string description,
            Exception exception,
            string additional)
        {
            var logEntry = new LogEntry(
                logType,
                description,
                exception,
                additional);
            SaveLogEntry(logEntry);
        }

        private void SaveLogEntry(LogEntry logEntry)
        {
            _logRepository.Save(logEntry);
        }
    }
}