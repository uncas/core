namespace Uncas.Core.Logging
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Repository for saving log entries.
    /// </summary>
    public interface ILogRepository
    {
        /// <summary>
        /// Saves the specified log entry.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        void Save(LogEntry logEntry);

        /// <summary>
        /// Gets the log entries since a specified time.
        /// </summary>
        /// <param name="from">The start time.</param>
        /// <returns>A list of log entries.</returns>
        IEnumerable<LogEntry> GetLogEntries(DateTime from);
    }
}