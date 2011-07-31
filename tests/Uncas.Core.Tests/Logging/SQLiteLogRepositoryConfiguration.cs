namespace Uncas.Core.Tests.Logging
{
    using System.Data.Common;
    using System.Data.SQLite;
    using Uncas.Core.Logging;

    public class SQLiteLogRepositoryConfiguration : ILogRepositoryConfiguration
    {
        private readonly string _fileName;

        public SQLiteLogRepositoryConfiguration(string fileName)
        {
            _fileName = fileName;
        }

        public string ConnectionString
        {
            get { return string.Format("Data Source={0};Version=3;", _fileName); }
        }

        public DbProviderFactory Factory
        {
            get { return SQLiteFactory.Instance; }
        }
    }
}
