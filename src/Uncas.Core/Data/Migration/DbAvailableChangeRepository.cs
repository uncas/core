namespace Uncas.Core.Data.Migration
{
    using System.Collections.Generic;

    /// <summary>
    /// Storage of available db changes.
    /// </summary>
    public class DbAvailableChangeRepository : IAvailableChangeRepository<DbChange>
    {
        /// <summary>
        /// Gets the available changes.
        /// </summary>
        /// <returns>A list of available changes.</returns>
        public IEnumerable<DbChange> GetAvailableChanges()
        {
            // TODO: Make real implementation here
            // depending on where the scripts are stored:
            var result = new List<DbChange>();
            result.Add(new DbChange("1", "CREATE TABLE ..."));
            result.Add(new DbChange("2", "ALTER TABLE ..."));
            result.Add(new DbChange("3", "DROP TABLE ..."));
            return result;
        }
    }
}
