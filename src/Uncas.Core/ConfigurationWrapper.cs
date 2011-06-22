using System.Configuration;

namespace Uncas.Core
{
    /// <summary>
    /// Wraps configuration.
    /// </summary>
    public static class ConfigurationWrapper
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <returns>The connection string.</returns>
        public static string GetConnectionString(
            string connectionStringName)
        {
            return ConfigurationManager
                .ConnectionStrings[connectionStringName]
                .ConnectionString;
        }
    }
}
