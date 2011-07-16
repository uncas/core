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
        string ConnectionString { get; }

        /// <summary>
        /// Gets the factory.
        /// </summary>
        DbProviderFactory Factory { get; }
    }
}