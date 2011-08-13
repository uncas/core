namespace Uncas.Core.Data.Migration
{
    using System;
    using System.Data.Common;
    using System.Diagnostics.CodeAnalysis;

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
        [SuppressMessage(
            "Microsoft.Security",
            "CA2100:Review SQL queries for security vulnerabilities",
            Justification =
                "The db migration requires sql to come from some external source - in this case it's a source that should be under control of the developer..."
            )]
        public void ApplyChange(DbChange change)
        {
            if (change == null)
            {
                throw new ArgumentNullException("change");
            }

            if (string.IsNullOrEmpty(change.ChangeScript))
            {
                throw new ArgumentException(
                    "The change must have a command text (ChangeScript property was null or empty).",
                    "change");
            }

            using (DbCommand command = CreateCommand())
            {
                command.CommandText = change.ChangeScript;
                ModifyData(command);
            }
        }
    }
}