namespace Uncas.Core.Logging
{
    using System.Data.Common;

    /// <summary>
    /// Configuration of the log repository.
    /// </summary>
    public interface ILogRepositoryConfiguration
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