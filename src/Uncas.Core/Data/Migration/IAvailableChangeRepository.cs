namespace Uncas.Core.Data.Migration
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Stores the available migration changes of a given type.
    /// </summary>
    /// <typeparam name="T">The type of the migration change to be retrieved from this repository.</typeparam>
    public interface IAvailableChangeRepository<T> where T : IMigrationChange
    {
        /// <summary>
        /// Gets the available changes.
        /// </summary>
        /// <returns>A list of available changes.</returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1024:UsePropertiesWhereAppropriate",
            Justification = "This involves looking up the values in some form of storage.")]
        IEnumerable<T> GetAvailableChanges();
    }
}