using System.Data.SqlClient;

namespace Uncas.Core.Data
{
    /// <summary>
    /// SQL db context.
    /// </summary>
    public class SqlDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDbContext"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlDbContext(string connectionString)
            : base(SqlClientFactory.Instance, connectionString)
        {
        }
    }
}