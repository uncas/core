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
        /// <param name="logDbContext">The log db context.</param>
        public LogRepository(ILogDbContext logDbContext)
            : base(GetFactory(logDbContext), GetConnectionString(logDbContext))
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

        private static DbProviderFactory GetFactory(
            ILogDbContext logDbContext)
        {
            if (logDbContext == null)
            {
                throw new ArgumentNullException("logDbContext");
            }

            return logDbContext.Factory;
        }

        private static string GetConnectionString(
            ILogDbContext logDbContext)
        {
            if (logDbContext == null)
            {
                throw new ArgumentNullException("logDbContext");
            }

            return logDbContext.ConnectionString;
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
    }

    /// <summary>
    /// Schema for log database.
    /// </summary>
    public class LogDbSchemaChangeRepository : IAvailableChangeRepository<DbChange>
    {
        private const string CreateLogEntryTableCommandText = @"
CREATE TABLE LogEntry
(
    Id integer PRIMARY KEY ASC
    , Created datetime
    , LogType integer
    , StackTrace text
    , FileName text
    , LineNumber integer
    , Description text
    , ExceptionType text
    , ExceptionMessage text
    , Additional text
    , ApplicationInfo text
    , ServiceId integer
)";

        private const string CreateLogEntryHttpStateTableCommandText = @"
CREATE TABLE LogEntryHttpState
(
    Id integer PRIMARY KEY ASC
    , LogEntryId integer
    , StatusCode integer
    , Url text
    , Referrer text
    , UserHostAddress text
    , Headers text
    , UserName text
)";

        /// <summary>
        /// Gets the available changes.
        /// </summary>
        /// <returns>A list of changes.</returns>
        public IEnumerable<DbChange> GetAvailableChanges()
        {
            var result = new List<DbChange>();
            result.Add(new DbChange("LogEntry-CreateTable", CreateLogEntryTableCommandText));
            result.Add(new DbChange("LogEntryHttpState-CreateTable", CreateLogEntryHttpStateTableCommandText));
            return result;
        }
    }
}