namespace Uncas.Core.Logging
{
    using System.Data.Common;

    /// <summary>
    /// The log database context.
    /// </summary>
    public interface ILogDbContext
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        string ConnectionString { get; }

        /// <summary>
        /// Gets the factory.
        /// </summary>
        /// <value>The database provider factory.</value>
        DbProviderFactory Factory { get; }
    }
}