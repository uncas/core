namespace Uncas.Core.Logging
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;

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
            Additional = additional;
            Created = SystemTime.Now();

            var stackTrace = new StackTrace(1, true);

            if (exception != null)
            {
                ExceptionMessage = exception.Message;
                ExceptionType = exception.GetType().ToString();
                stackTrace = new StackTrace(exception, true);
            }

            StackTrace = stackTrace.ToString();

            AssignFileNameAndLineNumber(stackTrace);
            AssignHttpState();
            AssignApplicationInfo();

            // TODO: Set ServiceId properly.
            ServiceId = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="logType">Type of the log.</param>
        /// <param name="description">The description.</param>
        /// <param name="created">The created.</param>
        /// <param name="additional">The additional.</param>
        /// <param name="exceptionType">Type of the exception.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="applicationInfo">The application info.</param>
        /// <param name="serviceId">The service id.</param>
        /// <param name="httpState">State of the HTTP.</param>
        internal LogEntry(
            int id,
            LogType logType,
            string description,
            DateTime created,
            string additional,
            string exceptionType,
            string exceptionMessage,
            string stackTrace,
            string fileName,
            int lineNumber,
            string applicationInfo,
            int serviceId,
            LogEntryHttpState httpState)
        {
            Id = id;
            LogType = logType;
            Description = description;
            Created = created;
            Additional = additional;
            ExceptionType = exceptionType;
            ExceptionMessage = exceptionMessage;
            StackTrace = stackTrace;
            FileName = fileName;
            LineNumber = lineNumber;
            ApplicationInfo = applicationInfo;
            ServiceId = serviceId;
            HttpState = httpState;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id of the log entry.</value>
        public int? Id { get; private set; }

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

        /// <summary>
        /// Gets the state of the HTTP.
        /// </summary>
        /// <value>
        /// The state of the HTTP.
        /// </value>
        public LogEntryHttpState HttpState { get; private set; }

        /// <summary>
        /// Gets the application info.
        /// </summary>
        /// <value>The application info.</value>
        public string ApplicationInfo { get; private set; }

        /// <summary>
        /// Gets the service id.
        /// </summary>
        /// <value>The service id.</value>
        public int ServiceId { get; private set; }

        /// <summary>
        /// Assigns the id.
        /// </summary>
        /// <param name="id">The id.</param>
        internal void AssignId(int id)
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException("id", "Id must be positive");
            }

            Id = id;
        }

        private static StackFrame GetStackFrame(StackTrace stackTrace)
        {
            var fileNamesToSkip = new[] { "LogEntry.cs", "Logger.cs" };
            foreach (StackFrame frame in stackTrace.GetFrames())
            {
                string fileName = frame.GetFileName();
                if (!fileNamesToSkip.Any(x => fileName.EndsWith(x)))
                {
                    return frame;
                }
            }

            return null;
        }

        private void AssignFileNameAndLineNumber(StackTrace stackTrace)
        {
            StackFrame frame = GetStackFrame(stackTrace);
            FileName = frame.GetFileName();
            LineNumber = frame.GetFileLineNumber();
        }

        private void AssignHttpState()
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                return;
            }

            HttpState = new LogEntryHttpState(httpContext);
        }

        private void AssignApplicationInfo()
        {
            // TODO: Set application info on log entry.
            ApplicationInfo = "NOT DEFINED YET";
        }
    }
}