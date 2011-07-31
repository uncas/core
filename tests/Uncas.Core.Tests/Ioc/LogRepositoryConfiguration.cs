namespace Uncas.Core.Tests.Ioc
{
    using System.Data.Common;
    using Uncas.Core.Logging;

    public class LogRepositoryConfiguration : ILogRepositoryConfiguration
    {
        public string ConnectionString
        {
            get { return null; }
        }

        public DbProviderFactory Factory
        {
            get { return null; }
        }
    }
}