namespace Uncas.Core.Logging
{
    using System;
    using System.Data.Common;
    using Uncas.Core.Data;

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
            InitializeDatabase();
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

            const string CommandText = @"
INSERT INTO LogEntry
(Created
    , StackTrace
    , FileName
    , LineNumber
    , Description
    , ExceptionType
    , ExceptionMessage
    , Additional)
VALUES
(@Created
    , @StackTrace
    , @FileName
    , @LineNumber
    , @Description
    , @ExceptionType
    , @ExceptionMessage
    , @Additional)
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
                command.CommandText = CommandText;
                ModifyData(
                    command);
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

        private void InitializeDatabase()
        {
            const string TableCountCommandText = @"
SELECT COUNT(*) FROM sqlite_master WHERE name = 'LogEntry'";
            using (var command = CreateCommand())
            {
                command.CommandText = TableCountCommandText;
                int tableCount = (int)GetScalar<long>(command);
                if (tableCount > 0)
                {
                    return;
                }
            }

            const string CreateTableCommandText = @"
CREATE TABLE LogEntry
(
    Id integer PRIMARY KEY ASC
    , Created datetime
    , StackTrace text
    , FileName text
    , LineNumber integer
    , Description text
    , ExceptionType text
    , ExceptionMessage text
    , Additional text
)";
            using (var command = CreateCommand())
            {
                command.CommandText = CreateTableCommandText;
                ModifyData(command);
            }
        }
    }
}