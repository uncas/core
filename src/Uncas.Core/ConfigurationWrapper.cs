using System.Configuration;

namespace Uncas.Core
{
    /// <summary>
    /// Wraps configuration.
    /// </summary>
    public static class ConfigurationWrapper
    {
        public static string GetConnectionString(
            string connectionStringName)
        {
            return ConfigurationManager
                .ConnectionStrings[connectionStringName]
                .ConnectionString;
        }
    }
}
