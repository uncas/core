namespace Uncas.Core.Data.Migration
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Stores which changes that have already been applied.
    /// </summary>
    public interface IAppliedChangeRepository
    {
        /// <summary>
        /// Gets the applied changes.
        /// </summary>
        /// <returns>A list of applied changes.</returns>
        [SuppressMessage(
           "Microsoft.Design",
           "CA1024:UsePropertiesWhereAppropriate",
           Justification = "This involves looking up the values in some form of storage.")]
        IEnumerable<IMigrationChange> GetAppliedChanges();

        /// <summary>
        /// Adds the applied change.
        /// </summary>
        /// <param name="change">The change.</param>
        void AddAppliedChange(IMigrationChange change);
    }
}