namespace Uncas.Core.Data.Migration
{
    using System.Data.Common;

    /// <summary>
    /// The target for db changes (mostly a database).
    /// </summary>
    public class DbTarget : DbContext, IMigrationTarget<DbChange>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbTarget"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="connectionString">The connection string.</param>
        public DbTarget(
            DbProviderFactory factory,
            string connectionString)
            : base(factory, connectionString)
        {
        }

        /// <summary>
        /// Applies the change.
        /// </summary>
        /// <param name="change">The change.</param>
        public void ApplyChange(DbChange change)
        {
            using (DbCommand command = CreateCommand())
            {
                command.CommandText = change.ChangeScript;
                ModifyData(command);
            }
        }
    }
}