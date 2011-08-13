namespace Uncas.Core.Data.Migration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Migrates changes in the most basic of ways.
    /// </summary>
    public class MigrationService : IMigrationService
    {
        /// <summary>
        /// Migrates the changes.
        /// </summary>
        /// <typeparam name="T">The type of the migration change.</typeparam>
        /// <param name="availableChangeRepository">The available change repository.</param>
        /// <param name="appliedChangeRepository">The applied change repository.</param>
        /// <param name="migrationTarget">The migration target.</param>
        public void Migrate<T>(
            IAvailableChangeRepository<T> availableChangeRepository,
            IAppliedChangeRepository appliedChangeRepository,
            IMigrationTarget<T> migrationTarget) where T : IMigrationChange
        {
            if (availableChangeRepository == null)
            {
                throw new ArgumentNullException("availableChangeRepository");
            }

            if (appliedChangeRepository == null)
            {
                throw new ArgumentNullException("appliedChangeRepository");
            }

            IEnumerable<T> availableChanges =
                availableChangeRepository.GetAvailableChanges();
            if (availableChanges.Count() == 0)
            {
                return;
            }

            IEnumerable<IMigrationChange> appliedChanges =
                appliedChangeRepository.GetAppliedChanges();
            foreach (T change in availableChanges)
            {
                if (!IsAlreadyApplied(appliedChanges, change))
                {
                    ApplyChange(
                        appliedChangeRepository,
                        change,
                        migrationTarget);
                }
            }
        }

        private static void ApplyChange<T>(
            IAppliedChangeRepository appliedChangeRepository,
            T change,
            IMigrationTarget<T> migrationTarget) where T : IMigrationChange
        {
            migrationTarget.ApplyChange(change);
            appliedChangeRepository.AddAppliedChange(change);
        }

        private static bool IsAlreadyApplied(
            IEnumerable<IMigrationChange> appliedChanges,
            IMigrationChange change)
        {
            return appliedChanges.Any(x => x.Id == change.Id);
        }
    }
}