namespace Uncas.Core.Logging
{
    using System;

    /// <summary>
    /// Default log repository.
    /// </summary>
    public class LogRepository : ILogRepository
    {
        /// <summary>
        /// Saves the specified log entry.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        public void Save(LogEntry logEntry)
        {
            throw new NotImplementedException();
        }
    }
}