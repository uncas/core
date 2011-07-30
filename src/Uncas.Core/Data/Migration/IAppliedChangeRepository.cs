namespace Uncas.Core.Data.Migration
{
    using System.Collections.Generic;

    /// <summary>
    /// Stores which changes that have already been applied.
    /// </summary>
    public interface IAppliedChangeRepository
    {
        /// <summary>
        /// Gets the applied changes.
        /// </summary>
        /// <returns>A list of applied changes.</returns>
        IEnumerable<IMigrationChange> GetAppliedChanges();

        /// <summary>
        /// Adds the applied change.
        /// </summary>
        /// <param name="change">The change.</param>
        void AddAppliedChange(IMigrationChange change);
    }
}