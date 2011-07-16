namespace Uncas.Core.Logging
{
    using System;

    /// <summary>
    /// Represents a log entry.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="description">The description.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="additional">The additional.</param>
        public LogEntry
            (LogType logType,
            string description, 
            Exception exception, 
            string additional)
        {
            LogType = logType;
            Description = description;
            Exception = exception;
            Additional = additional;
        }

        /// <summary>
        /// Gets the type of the log.
        /// </summary>
        /// <value>
        /// The type of the log.
        /// </value>
        public LogType LogType { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets the additional.
        /// </summary>
        public string Additional { get; private set; }
    }
}