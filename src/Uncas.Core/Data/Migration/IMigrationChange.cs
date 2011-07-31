namespace Uncas.Core.Data.Migration
{
    /// <summary>
    /// Represents a migration change.
    /// </summary>
    public interface IMigrationChange
    {
        /// <summary>
        /// Gets the id of the migration change.
        /// </summary>
        /// <value>The id of the migration change.</value>
        string Id { get; }
    }
}