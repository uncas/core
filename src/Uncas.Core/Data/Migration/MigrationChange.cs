namespace Uncas.Core.Data.Migration
{
    using Uncas.Core.Ioc;

    /// <summary>
    /// Represents a base migration change.
    /// </summary>
    [IocIgnore]
    public class MigrationChange : IMigrationChange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationChange"/> class.
        /// </summary>
        /// <param name="id">The id of the migration change.</param>
        public MigrationChange(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the id of the migration change.
        /// </summary>
        /// <value>The id of the migration change.</value>
        public string Id { get; private set; }
    }
}