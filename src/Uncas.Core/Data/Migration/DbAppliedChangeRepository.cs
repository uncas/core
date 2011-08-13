namespace Uncas.Core.Data.Migration
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;

    /// <summary>
    /// Storage of applied changes in a database.
    /// </summary>
    public class DbAppliedChangeRepository : DbContext, IAppliedChangeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbAppliedChangeRepository"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="connectionString">The connection string.</param>
        public DbAppliedChangeRepository(
            DbProviderFactory factory,
            string connectionString)
            : base(factory, connectionString)
        {
        }

        /// <summary>
        /// Adds the applied change.
        /// </summary>
        /// <param name="change">The change.</param>
        public void AddAppliedChange(IMigrationChange change)
        {
            if (change == null)
            {
                throw new ArgumentNullException("change");
            }

            if (string.IsNullOrEmpty(change.Id))
            {
                throw new ArgumentException(
                    "Invalid change: id is null or empty.",
                    "change");
            }

            InitializeDatabase();
            const string CreateTableCommandText =
                @"
INSERT INTO MigrationChange
(
    Id
    , DateApplied
)
VALUES
(
    @Id
    , @DateApplied
)";
            using (DbCommand command = CreateCommand())
            {
                AddParameter(command, "Id", change.Id);
                AddParameter(command, "DateApplied", SystemTime.Now());
                command.CommandText = CreateTableCommandText;
                ModifyData(command);
            }
        }

        /// <summary>
        /// Gets the applied changes.
        /// </summary>
        /// <returns>
        /// A list of applied changes.
        /// </returns>
        public IEnumerable<IMigrationChange> GetAppliedChanges()
        {
            InitializeDatabase();
            using (DbCommand command = CreateCommand())
            {
                command.CommandText = "SELECT * FROM MigrationChange";
                return GetObjects(command, MapToObject);
            }
        }

        private static IMigrationChange MapToObject(DbDataReader reader)
        {
            return new MigrationChange((string)reader["Id"]);
        }

        private void InitializeDatabase()
        {
            // TODO: Decouple from SQLite:
            const string TableCountCommandText = @"
SELECT COUNT(*) FROM sqlite_master WHERE name = 'MigrationChange'";
            using (DbCommand command = CreateCommand())
            {
                command.CommandText = TableCountCommandText;
                var tableCount = (int)GetScalar<long>(command);
                if (tableCount > 0)
                {
                    return;
                }
            }

            const string CreateTableCommandText =
                @"
CREATE TABLE MigrationChange
(
    ChangeNumber integer PRIMARY KEY ASC
    , Id text UNIQUE
    , DateApplied datetime
)";
            using (DbCommand command = CreateCommand())
            {
                command.CommandText = CreateTableCommandText;
                ModifyData(command);
            }
        }
    }
}