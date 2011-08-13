namespace Uncas.Core.Logging
{
    using System.Collections.Generic;
    using Uncas.Core.Data.Migration;

    /// <summary>
    /// Schema for log database.
    /// </summary>
    public class LogDbSchemaChangeRepository : IAvailableChangeRepository<DbChange>
    {
        private const string CreateLogEntryTableCommandText =
            @"
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

        private const string CreateLogEntryHttpStateTableCommandText =
            @"
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