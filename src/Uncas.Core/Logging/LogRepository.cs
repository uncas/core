namespace Uncas.Core.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using Uncas.Core.Data;
    using Uncas.Core.Data.Migration;

    /// <summary>
    /// Default log repository.
    /// </summary>
    public class LogRepository : DbContext, ILogRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogRepository"/> class.
        /// </summary>
        /// <param name="logRepositoryConfiguration">The log repository configuration.</param>
        public LogRepository(ILogRepositoryConfiguration logRepositoryConfiguration)
            : base(GetFactory(logRepositoryConfiguration), GetConnectionString(logRepositoryConfiguration))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogRepository"/> class.
        /// </summary>
        /// <param name="logDbContext">The log db context.</param>
        [Obsolete("Use overload with ILogRepositoryConfiguration instead.")]
        public LogRepository(ILogDbContext logDbContext)
            : this(logDbContext as ILogRepositoryConfiguration)
        {
        }

        /// <summary>
        /// Saves the specified log entry.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        public void Save(LogEntry logEntry)
        {
            if (logEntry == null)
            {
                throw new ArgumentNullException(
                    "logEntry",
                    "Log entry must be given.");
            }

            SaveLogEntry(logEntry);
            logEntry.AssignId(GetId());
            if (logEntry.HttpState != null)
            {
                SaveHttpState(logEntry);
            }
        }

        /// <summary>
        /// Gets the log entries since a specified time.
        /// </summary>
        /// <param name="from">The start time.</param>
        /// <returns>
        /// A list of log entries.
        /// </returns>
        public IEnumerable<LogEntry> GetLogEntries(DateTime from)
        {
            const string commandText = @"
SELECT * 
FROM LogEntry
WHERE @From <= Created
    AND Created <= @To
ORDER BY Created DESC;
";
            using (DbCommand command = CreateCommand())
            {
                AddParameter(command, "From", from);
                AddParameter(command, "To", SystemTime.Now());
                command.CommandText = commandText;
                return GetObjects<LogEntry>(command, MapToLogEntry);
            }
        }

        /// <summary>
        /// Migrates the schema for the underlying database.
        /// </summary>
        public void MigrateSchema()
        {
            var availableChangeRepository = new LogDbSchemaChangeRepository();
            var appliedChangeRepository =
                new DbAppliedChangeRepository(
                    Factory,
                    ConnectionString);
            var migrationTarget =
                new DbTarget(
                    Factory,
                    ConnectionString);
            var service = new MigrationService();
            service.Migrate<DbChange>(
                availableChangeRepository,
                appliedChangeRepository,
                migrationTarget);
        }

        private static LogEntry MapToLogEntry(DbDataReader reader)
        {
            return new LogEntry(
                (int)(long)reader["Id"],
                (LogType)(int)(long)reader["LogType"],
                GetString(reader, "Description"),
                GetDate(reader, "Created"),
                GetString(reader, "Additional"),
                GetString(reader, "ExceptionType"),
                GetString(reader, "ExceptionMessage"),
                GetString(reader, "StackTrace"),
                GetString(reader, "FileName"),
                (int)(long)reader["LineNumber"],
                GetString(reader, "ApplicationInfo"),
                (int)(long)reader["ServiceId"],
                null);
        }

        private static DbProviderFactory GetFactory(
            ILogRepositoryConfiguration logRepositoryConfiguration)
        {
            if (logRepositoryConfiguration == null)
            {
                throw new ArgumentNullException("logRepositoryConfiguration");
            }

            return logRepositoryConfiguration.Factory;
        }

        private static string GetConnectionString(
            ILogRepositoryConfiguration logRepositoryConfiguration)
        {
            if (logRepositoryConfiguration == null)
            {
                throw new ArgumentNullException("logRepositoryConfiguration");
            }

            return logRepositoryConfiguration.ConnectionString;
        }

        private int GetId()
        {
            const string CommandText = @"
SELECT MAX(Id) FROM LogEntry
";
            using (var command = CreateCommand())
            {
                command.CommandText = CommandText;
                return (int)GetScalar<long>(
                    command);
            }
        }

        private void SaveLogEntry(LogEntry logEntry)
        {
            const string CommandText = @"
INSERT INTO LogEntry
(Created
    , LogType
    , StackTrace
    , FileName
    , LineNumber
    , Description
    , ExceptionType
    , ExceptionMessage
    , Additional
    , ApplicationInfo
    , ServiceId)
VALUES
(@Created
    , @LogType
    , @StackTrace
    , @FileName
    , @LineNumber
    , @Description
    , @ExceptionType
    , @ExceptionMessage
    , @Additional
    , @ApplicationInfo
    , @ServiceId)
";
            using (var command = CreateCommand())
            {
                AddParameter(command, "Created", logEntry.Created);
                AddParameter(command, "StackTrace", logEntry.StackTrace);
                AddParameter(command, "FileName", logEntry.FileName);
                AddParameter(command, "LineNumber", logEntry.LineNumber);
                AddParameter(command, "Description", logEntry.Description);
                AddParameter(command, "ExceptionType", logEntry.ExceptionType);
                AddParameter(command, "ExceptionMessage", logEntry.ExceptionMessage);
                AddParameter(command, "Additional", logEntry.Additional);
                AddParameter(command, "ApplicationInfo", logEntry.ApplicationInfo);
                AddParameter(command, "ServiceId", logEntry.ServiceId);
                AddParameter(command, "LogType", logEntry.LogType);
                command.CommandText = CommandText;
                ModifyData(
                    command);
            }
        }

        private void SaveHttpState(LogEntry logEntry)
        {
            LogEntryHttpState logEntryHttpState = logEntry.HttpState;
            const string CommandText = @"
INSERT INTO LogEntryHttpState
(LogEntryId
    , StatusCode
    , Url
    , Referrer
    , UserHostAddress
    , Headers
    , UserName)
VALUES
(@LogEntryId
    , @StatusCode
    , @Url
    , @Referrer
    , @UserHostAddress
    , @Headers
    , @UserName)
";
            using (var command = CreateCommand())
            {
                AddParameter(command, "LogEntryId", logEntry.Id);
                AddParameter(command, "StatusCode", logEntryHttpState.StatusCode);
                AddParameter(command, "Url", logEntryHttpState.Url);
                AddParameter(command, "Referrer", logEntryHttpState.Referrer);
                AddParameter(command, "UserHostAddress", logEntryHttpState.UserHostAddress);
                AddParameter(command, "Headers", logEntryHttpState.Headers);
                AddParameter(command, "UserName", logEntryHttpState.UserName);
                command.CommandText = CommandText;
                ModifyData(
                    command);
            }
        }
    }
}