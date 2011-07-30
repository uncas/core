namespace Uncas.Core.Data.Migration
{
    /// <summary>
    /// A target for applying migration changes.
    /// </summary>
    /// <typeparam name="T">The type of the migration change.</typeparam>
    public interface IMigrationTarget<T> where T : IMigrationChange
    {
        /// <summary>
        /// Applies the change.
        /// </summary>
        /// <param name="change">The change.</param>
        void ApplyChange(T change);
    }
}