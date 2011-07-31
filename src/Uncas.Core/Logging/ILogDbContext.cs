namespace Uncas.Core.Logging
{
    using System;

    /// <summary>
    /// The log database context.
    /// </summary>
    [Obsolete("Use ILogRepositoryConfiguration instead.")]
    public interface ILogDbContext : ILogRepositoryConfiguration
    {
    }
}