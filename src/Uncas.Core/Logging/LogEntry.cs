namespace Uncas.Core.Logging
{
    using System;
    using System.Diagnostics;

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
        public LogEntry(
            LogType logType,
            string description,
            Exception exception,
            string additional)
        {
            LogType = logType;
            Description = description;
            Exception = exception;
            Additional = additional;

            Created = SystemTime.Now();
            var stackTrace = new StackTrace(1, true);
            if (Exception != null)
            {
                ExceptionMessage = Exception.Message;
                ExceptionType = Exception.GetType().ToString();
            }

            StackTrace = stackTrace.ToString();
            AssignFileNameAndLineNumber(stackTrace);
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
        /// <value>The description.</value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets the additional.
        /// </summary>
        /// <value>Any additional information.</value>
        public string Additional { get; private set; }

        /// <summary>
        /// Gets the created date and time.
        /// </summary>
        /// <value>The time when the log entry was created.</value>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Gets the stack trace.
        /// </summary>
        /// <value>The stack trace.</value>
        public string StackTrace { get; private set; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the line number.
        /// </summary>
        /// <value>The line number.</value>
        public int LineNumber { get; private set; }

        /// <summary>
        /// Gets the type of the exception.
        /// </summary>
        /// <value>
        /// The type of the exception.
        /// </value>
        public string ExceptionType { get; private set; }

        /// <summary>
        /// Gets the exception message.
        /// </summary>
        /// <value>The message of the exception.</value>
        public string ExceptionMessage { get; private set; }

        private void AssignFileNameAndLineNumber(StackTrace stackTrace)
        {
            var frame = stackTrace.GetFrame(0);
            FileName = frame.GetFileName();
            LineNumber = frame.GetFileLineNumber();
        }
    }
}