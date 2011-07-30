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
                return GetObjects(command, MapToObject);
            }
        }

        /// <summary>
        /// Adds the applied change.
        /// </summary>
        /// <param name="change">The change.</param>
        public void AddAppliedChange(IMigrationChange change)
        {
            InitializeDatabase();
        }

        private static void InitializeDatabase()
        {
            // TODO: Implement migration db initialization...
            throw new NotImplementedException();
        }

        private static IMigrationChange MapToObject(DbDataReader reader)
        {
            return new MigrationChange((string)reader["Id"]);
        }
    }
}
