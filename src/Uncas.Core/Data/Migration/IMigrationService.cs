namespace Uncas.Core.Data.Migration
{
    /// <summary>
    /// Service for arranging the different parts of migrating changes.
    /// </summary>
    public interface IMigrationService
    {
        /// <summary>
        /// Migrates the changes.
        /// </summary>
        /// <typeparam name="T">The type of the migration change.</typeparam>
        /// <param name="availableChangeRepository">The available change repository.</param>
        /// <param name="appliedChangeRepository">The applied change repository.</param>
        /// <param name="migrationTarget">The migration target.</param>
        void Migrate<T>(
            IAvailableChangeRepository<T> availableChangeRepository,
            IAppliedChangeRepository appliedChangeRepository,
            IMigrationTarget<T> migrationTarget)
            where T : IMigrationChange;
    }
}