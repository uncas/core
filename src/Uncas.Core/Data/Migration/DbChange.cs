namespace Uncas.Core.Data.Migration
{
    /// <summary>
    /// Represents a database migration change.
    /// </summary>
    public class DbChange : MigrationChange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbChange"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="changeScript">The change script.</param>
        public DbChange(
            string id,
            string changeScript)
            : base(id)
        {
            ChangeScript = changeScript;
        }

        /// <summary>
        /// Gets the change script.
        /// </summary>
        /// <value>The change script.</value>
        public string ChangeScript { get; private set; }
    }
}
