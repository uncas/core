namespace Uncas.Core
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Wraps configuration.
    /// </summary>
    public static class ConfigurationWrapper
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <returns>
        /// The connection string.
        /// </returns>
        public static string GetConnectionString(
            string connectionStringName)
        {
            var connectionStringObject =
                ConfigurationManager
                .ConnectionStrings[connectionStringName];
            if (connectionStringObject == null)
            {
                throw new ArgumentException(
                    "No connection string with the name " + connectionStringName,
                    "connectionStringName");
            }

            return connectionStringObject.ConnectionString;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The connection string.
        /// </returns>
        public static string GetConnectionString(
            string connectionStringName,
            string defaultValue)
        {
            var connectionStringObject =
                ConfigurationManager
                .ConnectionStrings[connectionStringName];
            if (connectionStringObject == null)
            {
                return defaultValue;
            }

            return connectionStringObject.ConnectionString;
        }
    }
}
