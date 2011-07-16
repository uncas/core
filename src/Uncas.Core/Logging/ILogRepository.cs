namespace Uncas.Core.Logging
{
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
    }
}