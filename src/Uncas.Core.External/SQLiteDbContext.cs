namespace Uncas.Core.External
{
    using System.Data.SQLite;
    using System.Diagnostics.CodeAnalysis;
    using Uncas.Core.Data;

    /// <summary>
    /// Db context for SQLite.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1709:IdentifiersShouldBeCasedCorrectly",
        MessageId = "Db",
        Justification = "Follows how 'Db' is cased in .NET framework library System.Data.")]
    public abstract class SQLiteDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteDbContext"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        protected SQLiteDbContext(string connectionString)
            : base(SQLiteFactory.Instance, connectionString)
        {
        }
    }
}